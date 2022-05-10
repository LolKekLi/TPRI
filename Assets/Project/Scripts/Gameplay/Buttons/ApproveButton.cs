using System;

namespace Project
{
    public abstract class ApproveButton : BoardButton
    {
        public static event Action<bool> ApproveClicked = delegate {  };

        protected override void OnClick()
        {
            ApproveClicked(IsOpponent);
        }
    }
}