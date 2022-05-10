using System;

namespace Project
{
    public abstract class RequestButton : BoardButton
    {
        public static event Action<bool> RequestClicked = delegate {  };
        
        protected override void OnClick()
        {
            RequestClicked(IsOpponent);
        }
    }
}