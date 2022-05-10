using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.UI
{
    public class UITradedItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public static event Action<UITradedItem, Vector2> ItemSelected = delegate { };
        public static event Action<UITradedItem, Vector2> ItemReleased = delegate { };
        public static event Action<UITradedItem> Clicked = delegate { };

        [SerializeField]
        private Image _backgroundIcon = null;

        [SerializeField]
        private Image _backgroundStrokeIcon = null;

        [SerializeField]
        private Image _fxIcon = null;

        [SerializeField]
        private Image _itemIcon = null;

        [SerializeField]
        private UICounter _counter = null;

        [SerializeField]
        private GameObject[] _starGroups = null;

        [Header("Buy Group")]
        [SerializeField]
        private GameObject _buyGroup = null;
        
        [SerializeField]
        private Button _buyButton = null;

        [SerializeField]
        private TextMeshProUGUI _countLabel = null;

        [SerializeField]
        private TextMeshProUGUI _priceLabel = null;

        [SerializeField]
        private SelfSingleTweenController _buyController = null;

        private ScrollRect _scrollRect = null;
        
        public TradedItemPreset ItemPreset
        {
            get;
            private set;
        }

        private bool IsItemAvailable
        {
            get => User.Current.GetTradeItemCount(ItemPreset.ItemType) > 0;
        }

        private void Start()
        {
            _buyButton.onClick.AddListener(OnBuyButtonClick);
        }

        private void OnEnable()
        {
            User.Changed += User_Changed;
        }

        private void OnDisable()
        {
            User.Changed -= User_Changed;
        }

        public void Setup(ScrollRect scrollRect, TradedItemPreset preset)
        {
            gameObject.SetActive(true);
            _scrollRect = scrollRect;
            
            ItemPreset = preset;
            var rarity = preset.RarityType;
            var rarityPreset = AssetsManager.Instance.RaritySettings.GetPreset(rarity);
            var priceSettings = preset.PriceSettings;

            _backgroundIcon.color = rarityPreset.BackgroundColor;
            _backgroundStrokeIcon.color = rarityPreset.BackgroundStrokeColor;
            _counter.Setup(rarityPreset.CounterStrokeColor);
            
            _fxIcon.gameObject.SetActive(rarityPreset.FxSprite != null);
            if (_fxIcon.gameObject.activeSelf)
            {
                _fxIcon.sprite = rarityPreset.FxSprite;
            }

            _itemIcon.sprite = preset.ItemSprite;
            _itemIcon.SetNativeSize();

            _countLabel.text = $"GET {priceSettings.RewardCount}";
            _priceLabel.text = $"{priceSettings.Price}";
            
            Refresh();
        }

        private void Refresh()
        {
            int itemCount = User.Current.GetTradeItemCount(ItemPreset.ItemType);
            _counter.Refresh(itemCount);
            
            _buyGroup.SetActive(itemCount <= 0);

            RefreshStars();
        }

        private void RefreshStars()
        {
            var rarity = ItemPreset.RarityType;
            int rarityIndex = (int)rarity - 1;
            for (int i = 0; i < _starGroups.Length; i++)
            {
                _starGroups[i].SetActive(false);
            }

            if (_buyGroup.activeSelf)
            {
                return;
            }
            
            if (rarityIndex >= 0 && rarityIndex < _starGroups.Length)
            {
                _starGroups[rarityIndex].SetActive(true);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (IsItemAvailable)
            {
                TradeController.Instance.MoveItem(eventData.position);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var delta = eventData.delta;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                eventData.pointerDrag = _scrollRect.gameObject;
                EventSystem.current.SetSelectedGameObject(_scrollRect.gameObject);
                _scrollRect.OnInitializePotentialDrag(eventData);
                _scrollRect.OnBeginDrag(eventData);
            }
            else
            {
                if (IsItemAvailable)
                {
                    ItemSelected(this, eventData.position);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsItemAvailable)
            {
                ItemReleased(this, eventData.position);
            }
        }

        private void OnBuyButtonClick()
        {
            UserManager.Instance.PurchaseItem(ItemPreset.ItemType, isSuccess =>
            {
                if (isSuccess)
                {
                    _buyController.Play();
                    
                    Refresh();
                }
            });
        }
        
        private void User_Changed(ItemType itemType)
        {
            if (itemType == ItemPreset.ItemType)
            {
                Refresh();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsItemAvailable)
            {
                Clicked(this);
            }
        }
    }
}