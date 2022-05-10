using UnityEngine;

namespace Project
{
    public class PlayerApproveButton : ApproveButton
    {
        protected override bool IsOpponent
        {
            get => false;
        }
        
        protected void OnEnable()
        {
            TradeController.ItemPlaced += TradeController_ItemPlaced;
        }

        protected void OnDisable()
        {
            TradeController.ItemPlaced -= TradeController_ItemPlaced;
        }

        private void OnMouseUpAsButton()
        {
            Click();
        }

        private void TradeController_ItemPlaced(TradedItem item, bool isOpponent)
        {
            if (LocalConfig.BasicTutorialNeeded)
            {
                var position = GameUISystem.Instance.GetViewportPosition(transform.position);

                TutorialManager.FreeHand();
                TutorialManager.Instance.SetupHand(position, new Vector2(50, -75));
            }
        }
    }
}