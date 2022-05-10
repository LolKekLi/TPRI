namespace Project
{
    public class PlayerRequestButton : RequestButton
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