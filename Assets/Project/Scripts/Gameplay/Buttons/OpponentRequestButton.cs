namespace Project
{
    public class OpponentRequestButton : RequestButton
    {
        protected override bool IsOpponent
        {
            get => true;
        }
        
        private void OnEnable()
        {
            OpponentController.Decided += OpponentController_Decided;
        }

        private void OnDisable()
        {
            OpponentController.Decided -= OpponentController_Decided;
        }

        private void OpponentController_Decided(DecisionType type)
        {
            if (type == DecisionType.RequestMore)
            {
                Click();
            }
        }
    }
}