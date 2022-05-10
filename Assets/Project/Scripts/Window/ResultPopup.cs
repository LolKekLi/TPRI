using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Project.UI
{
    public class ResultPopup : Window
    {
        public static event Action ClaimClicked = delegate { };

        [SerializeField]
        private Image _emojiIcon = null;

        [SerializeField]
        private Sprite[] _goodTradeSprites = null;
        
        [SerializeField]
        private Sprite[] _badTradeSprites = null;

        [SerializeField]
        private TextMeshProUGUI _buttonMoneyLabel = null;

        [SerializeField]
        private TextMeshProUGUI _moneyLabel = null;
        
        [SerializeField]
        private TextMeshProUGUI _levelResultLabel = null;

        [SerializeField]
        private Button _claimButton = null;

        [SerializeField]
        private SelfSingleTweenController _appearController = null;

        [SerializeField]
        private SelfSingleTweenController _disappearController = null;

        private bool _isClaimed = false;
        
        public override bool IsPopup => true;

        protected override void Start()
        {
            base.Start();

            _claimButton.onClick.AddListener(OnClaimButtonClick);
        }

        public override void OnShow()
        {
            base.OnShow();

            _isClaimed = false;
            
            var tradeRate = TradeControllerExtensions.GetTradeRate();
            bool isGoodTrade = tradeRate >= 1f;
            
            _levelResultLabel.text = isGoodTrade ? "GOOD TRADE" : "TRY AGAIN";
            _buttonMoneyLabel.text = $"+{TradeControllerExtensions.GetTradedMoneyAmount()}";
            _moneyLabel.text = $"{User.Current.Coins}";
            _emojiIcon.sprite = isGoodTrade ? _goodTradeSprites.RandomElement() : _badTradeSprites.RandomElement();
            
            _appearController.Play();
        }

        private void OnClaimButtonClick()
        {
            if (_isClaimed)
            {
                return;
            }
            
            _disappearController.Play();
            
            this.InvokeWithDelay(.5f, () =>
            {
                UISystem.ShowWindow<GameWindow>();
            });

            ClaimClicked();

            _isClaimed = true;
        }
    }
}