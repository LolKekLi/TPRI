using System;

namespace Project
{
    public abstract class RejectButton : BoardButton
    {
        public static event Action<bool> RejectClicked = delegate {  }; 
        
        protected override void OnClick()
        {
            RejectClicked(IsOpponent);
        }
    }
}