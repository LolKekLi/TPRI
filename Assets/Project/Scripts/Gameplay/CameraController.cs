using UnityEngine;
using Project.UI;

namespace Project
{
    public class CameraController : MonoBehaviour
    {
        private readonly string TradeMainViewKey = "TradeMainView";
        private readonly string FinishTradeViewKey = "FinishTradeView"; 
        
        [SerializeField]
        private Animator _animator = null;

        private void OnEnable()
        {
            TradeController.TradeStarted += TradeController_TradeStarted;
            
            ResultPopup.ClaimClicked += ResultPopup_ClaimClicked;
        }

        private void OnDisable()
        {
            TradeController.TradeStarted -= TradeController_TradeStarted;
            
            ResultPopup.ClaimClicked -= ResultPopup_ClaimClicked;
        }

        private void TradeController_TradeStarted()
        {
            _animator.SetTrigger(FinishTradeViewKey);
        }

        private void ResultPopup_ClaimClicked()
        {
            _animator.SetTrigger(TradeMainViewKey);
        }
    }
}