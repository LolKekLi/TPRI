using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

namespace Project.UI
{
    public class GameWindow : Window
    {
        private const float ArrowMaxOffset = 300f;
        private const float ValueBarLerpOffset = 0.5f;

        [SerializeField]
        private AnimationCurve _arrowCurve = null;
        
        [SerializeField]
        private float _effectTime = .5f;
        
        [SerializeField]
        private RectTransform _arrowRectTransform = null;

        [SerializeField]
        private Transform _categoryButtonParent = null;

        [SerializeField]
        private UICategoryTab _categoryTabPrefab = null;

        [SerializeField]
        private UIScrollableTab _scrollableTab = null;

        [SerializeField]
        private TextMeshProUGUI _coinsLabel = null;

        [SerializeField]
        private SelfSingleTweenController _coinController = null;

        [SerializeField]
        private SelfSingleTweenController _appearController = null;

        [SerializeField]
        private SelfSingleTweenController _disappearController = null;

        private List<UICategoryTab> _categoryTabs = new List<UICategoryTab>();
        private Coroutine _arrowTransformCor = null;
        
        public override bool IsPopup => false;

        protected override void OnEnable()
        {
            base.OnEnable();

            TradeController.ItemPlaced += TradeController_ItemPlaced;
            UICategoryTab.TabClicked += UICategoryTab_TabClicked;
            
            UITradedItem.ItemSelected += UITradedItem_ItemSelected;
            TradeController.PlaceFailed += TradeController_PlaceFailed;
            TradeController.TradeStarted += TradeController_TradeStarted;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            TradeController.ItemPlaced -= TradeController_ItemPlaced;
            UICategoryTab.TabClicked -= UICategoryTab_TabClicked;
            
            UITradedItem.ItemSelected -= UITradedItem_ItemSelected;
            TradeController.PlaceFailed -= TradeController_PlaceFailed;
            TradeController.TradeStarted -= TradeController_TradeStarted;
        }

        public override void Preload()
        {
            base.Preload();

            var categories = (CategoryType[]) Enum.GetValues(typeof(CategoryType));

            for (int i = 0; i < categories.Length; i++)
            {
                var tab = Instantiate(_categoryTabPrefab, Vector3.zero, Quaternion.identity, _categoryButtonParent);
                tab.Setup(categories[i]);
                _categoryTabs.Add(tab);
            }
            
            _categoryTabs[0].Select(true);
            
            _scrollableTab.Prepare();
            _scrollableTab.Setup(CategoryType.Styling);
        }

        public override void OnShow()
        {
            base.OnShow();
            
            CalculateArrowPosition(.5f, true);

            _appearController.Play();

            this.InvokeWithDelay(1f, () =>
            {
                CheckTutorial();
            });
        }

        public override void Refresh()
        {
            base.Refresh();

            _coinsLabel.text = $"{User.Current.Coins}";
            _coinController.Play();
        }

        private void CheckTutorial()
        {
            if (TradeController.Instance.PlayerItems.Count <= 0 && LocalConfig.BasicTutorialNeeded)
            {
                this.InvokeWithDelay(0.25f, () =>
                {
                    var item = _scrollableTab.GetFirstItem();
                    var board = PlayableBoard.Instance;
            
                    var startPos = UISystem.Instance.Camera.WorldToViewportPoint(item.transform.position);
                    var finalPos = GameUISystem.Instance.GetViewportPosition(board.transform.position.ChangeZ(board.transform.position.z - 1f));
            
                    TutorialManager.Instance.SpawnAnimatedHand(startPos, finalPos);
                });
            }
        }

        private void CalculateArrowPosition(float progress, bool isImmediately)
        {
            var finalYPos = Mathf.Lerp(-ArrowMaxOffset, ArrowMaxOffset, progress);
            
            if (_arrowTransformCor != null)
            {
                StopCoroutine(_arrowTransformCor);
                _arrowTransformCor = null;
            }
            
            if (isImmediately)
            {
                _arrowRectTransform.anchoredPosition = new Vector2(_arrowRectTransform.anchoredPosition.x, finalYPos);
            }
            else
            {
                _arrowTransformCor = StartCoroutine(ArrowTransformCor(finalYPos));
            }
        }

        private IEnumerator ArrowTransformCor(float finalYPos)
        {
            float time = 0f;
            float progress = 0f;
            float startPos = _arrowRectTransform.anchoredPosition.y;
            
            while (time < _effectTime)
            {
                yield return null;

                time += Time.deltaTime;
                progress = time / _effectTime;

                _arrowRectTransform.anchoredPosition =
                    _arrowRectTransform.anchoredPosition.ChangeY(Mathf.Lerp(startPos, finalYPos, _arrowCurve.Evaluate(progress)));
            }
        }

        private void TradeController_ItemPlaced(TradedItem item, bool isOpponent)
        {
            var opponentItems = TradeController.Instance.OpponentItems;
            var playerItems = TradeController.Instance.PlayerItems;

            var opponentItemsValue = opponentItems.Sum(item => item.Value);
            var playerItemsValue = playerItems.Sum(item => item.Value);
            var playerValuePercent = opponentItemsValue / playerItemsValue;

            CalculateArrowPosition(playerValuePercent - ValueBarLerpOffset,
                false);
        }

        private void UICategoryTab_TabClicked(CategoryType category)
        {
            _scrollableTab.Setup(category);
        }

        private void UITradedItem_ItemSelected(UITradedItem arg1, Vector2 arg2)
        {
            if (LocalConfig.BasicTutorialNeeded)
            {
                TutorialManager.FreeHand();
            }
        }

        private void TradeController_PlaceFailed()
        {
            CheckTutorial();
        }

        private void TradeController_TradeStarted()
        {
            _disappearController.Play();
            
            if (LocalConfig.BasicTutorialNeeded)
            {
                TutorialManager.FreeHand();
                LocalConfig.BasicTutorialNeeded = false;
            }
        }
    }
}