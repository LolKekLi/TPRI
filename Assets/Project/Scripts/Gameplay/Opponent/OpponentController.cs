using System;
using UnityEngine;

namespace Project
{
    public class OpponentController : MonoBehaviour
    {
        public static event Action<DecisionType> Decided = delegate { };

        [SerializeField]
        private Transform[] _placePoints = null;

        private DecisionType _lastDecision = DecisionType.Default;
        
        private void OnEnable()
        {
            ApproveButton.ApproveClicked += ApproveButton_ApproveClicked;
            RequestButton.RequestClicked += AddMoreButton_RequestClicked;
            
            TradeController.ItemPlaced += TradeController_ItemPlaced;
        }

        private void OnDisable()
        {
            ApproveButton.ApproveClicked -= ApproveButton_ApproveClicked;
            RequestButton.RequestClicked -= AddMoreButton_RequestClicked;
            
            TradeController.ItemPlaced -= TradeController_ItemPlaced;
        }

        private void UpdateLogic()
        {
            int totalAmount = User.Current.GetTotalAmount();

            if (_lastDecision != DecisionType.RequestMore || totalAmount == 0)
            {
                _lastDecision = OpponentDecisionMaker.GetDecision(TradeController.Instance.LastPlayerAction);
            }

            Decided(_lastDecision);
        }

        private void AddMoreButton_RequestClicked(bool isOpponent)
        {
            if (!isOpponent)
            {
                this.InvokeWithDelay(AssetsManager.Instance.OpponentSettings.DelayBeforeReaction, () =>
                {
                    UpdateLogic();
                });
            }
        }

        private void ApproveButton_ApproveClicked(bool isOpponent)
        {
            if (!isOpponent && TradeController.Instance.LastOpponentAction != DecisionType.Accept)
            {
                this.InvokeWithDelay(AssetsManager.Instance.OpponentSettings.DelayBeforeReaction, () => 
                {
                    UpdateLogic(); 
                });
            }
        }

        private void TradeController_ItemPlaced(TradedItem item, bool isOpponent)
        {
            if (!isOpponent)
            {
                _lastDecision = DecisionType.Default;
                
                this.InvokeWithDelay(AssetsManager.Instance.OpponentSettings.DelayBeforeReaction, () =>
                {
                    UpdateLogic();
                });
            }
        }
    }
}