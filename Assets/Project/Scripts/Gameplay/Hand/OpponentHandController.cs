using System;
using UnityEngine;

namespace Project
{
    public class OpponentHandController : HandController
    {
        public static event Action<TradedItem> ItemPlaced = delegate { }; 
        
        [SerializeField]
        private Transform _spawnPoint = null;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            OpponentController.Decided += OpponentController_Decided;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            OpponentController.Decided -= OpponentController_Decided;
        }
        
        public void PlaceItem()
        {
            var position = _spawnPoint.position;
            
            var itemType = OpponentDecisionMaker.GetPlaceableItem();
            
            var item = PoolManager.Instance.Get<TradedItem>(
                AssetsManager.GetTradedPreset(itemType).Item,
                position, Quaternion.identity, TradeController.Instance.OpponentItemGroup);
            
            ItemPlaced(item);
        }

        private void OpponentController_Decided(DecisionType type)
        {
            switch (type)
            {
                case DecisionType.AddMore:
                    Play(HandActionType.AddItem);
                    break;
                
                case DecisionType.Accept:
                    Play(HandActionType.Trade);
                    break;
                
                case DecisionType.RequestMore:
                case DecisionType.Deny:
                    Play(HandActionType.More);
                    break;
            }
        }
    }
}