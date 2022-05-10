namespace Project
{
    public class PlayerRejectButton : RejectButton
    {
        protected override bool IsOpponent
        {
            get => false;
        }

        private void OnMouseUpAsButton()
        {
            Click();
        }
    }
}