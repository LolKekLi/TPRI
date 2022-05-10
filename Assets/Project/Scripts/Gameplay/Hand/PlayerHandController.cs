namespace Project
{
    public class PlayerHandController : HandController
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            
            RequestButton.RequestClicked += AddMoreButton_RequestClicked;
            ApproveButton.ApproveClicked += ApproveButtonApproveClicked;
            RejectButton.RejectClicked += RejectButton_RejectClicked;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            RequestButton.RequestClicked -= AddMoreButton_RequestClicked;
            ApproveButton.ApproveClicked -= ApproveButtonApproveClicked;
            RejectButton.RejectClicked -= RejectButton_RejectClicked;
        }

        private void AddMoreButton_RequestClicked(bool isOpponent)
        {
            if (!isOpponent)
            {
                Play(HandActionType.More);
            }
        }

        private void ApproveButtonApproveClicked(bool isOpponent)
        {
            if (!isOpponent)
            {
                Play(HandActionType.Trade);
            }
        }

        private void RejectButton_RejectClicked(bool isOpponent)
        {
            if (!isOpponent)
            {
                Play(HandActionType.Reject);
            }
        }
    }
}