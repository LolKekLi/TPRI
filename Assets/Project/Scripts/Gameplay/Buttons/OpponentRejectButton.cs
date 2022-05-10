namespace Project
{
    public class OpponentRejectButton : RejectButton
    {
        protected override bool IsOpponent
        {
            get => true;
        }
    }
}