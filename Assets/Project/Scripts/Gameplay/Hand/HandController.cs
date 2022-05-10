using UnityEngine;

namespace Project
{
    public abstract class HandController : MonoBehaviour
    {
        private readonly string MoreKey = "More";
        private readonly string TradeKey = "Trade";
        private readonly string DenyKey = "Deny";
        private readonly string FinalTakeKey = "FinalTake";
        private readonly string OpponentAddItemKey = "OpponentAddItem";

        [SerializeField]
        protected Animator _rightHandAnimator = null;
        
        [SerializeField]
        protected Animator _leftHandAnimator = null;

        protected virtual void OnEnable()
        {
            TradeController.BoardRotated += TradeController_BoardRotated;
        }

        protected virtual void OnDisable()
        {
            TradeController.BoardRotated -= TradeController_BoardRotated;
        }

        private void Animate(string key, bool isBothHands = false)
        {
            _rightHandAnimator.SetTrigger(key);

            if (isBothHands)
            {
                _leftHandAnimator.SetTrigger(key);
            }
        }
        
        protected void Play(HandActionType type)
        {
            switch (type)
            {
                case HandActionType.More:
                    Animate(MoreKey);
                    break;
                
                case HandActionType.Trade:
                    Animate(TradeKey);
                    break;
                
                case HandActionType.FinalTake:
                    Animate(FinalTakeKey, true);
                    break;
                
                case HandActionType.Reject:
                    Animate(DenyKey);
                    break;
                
                case HandActionType.AddItem:
                    Animate(OpponentAddItemKey);
                    break;
                
                default:
                    Debug.LogError($"Not found case for type: {type}");
                    break;
            }
        }

        private void TradeController_BoardRotated()
        {
            Play(HandActionType.FinalTake);
        }
    }
}