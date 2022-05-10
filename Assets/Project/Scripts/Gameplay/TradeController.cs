using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.UI;
using UnityEngine;

namespace Project
{
    public struct UIPointer
    {
        public Ray ray;
        public RaycastHit? raycast;
    }
    
    public class TradeController : MonoBehaviour
    {
        public static event Action<TradedItem, bool> ItemPlaced = delegate { };
        public static event Action PlaceFailed = delegate { };
        public static event Action BoardRotated = delegate { };
        public static event Action TradeStarted = delegate { };
        public static event Action<List<TradedItem>> Traded = delegate { };

        [SerializeField]
        private LayerMask _floorLayer = default;
        [SerializeField]
        private float _sphereCastRadius = 1;
        [SerializeField]
        private float _dragSpeed = 1f;

        [SerializeField]
        private Transform _rotateGroup = null;

        [SerializeField]
        private Transform _playerItemGroup = null;

        [SerializeField]
        private GameObject _confettiFx = null;

        [SerializeField]
        private Transform _playerClickSpawnPoint = null;
        
        [field: SerializeField]
        public Transform OpponentItemGroup
        {
            get;
            private set;
        }
        
        private Vector3 _expectedPos = Vector3.zero;
        
        private TradedItem _selectedItem = null;
        private Coroutine _tradeRotateCor = null;

        public PlayerActionType LastPlayerAction
        {
            get;
            private set;
        }

        public DecisionType LastOpponentAction
        {
            get;
            private set;
        }

        public List<TradedItem> PlayerItems
        {
            get;
            private set;
        } = new List<TradedItem>();

        public List<TradedItem> OpponentItems
        {
            get;
            private set;
        } = new List<TradedItem>();
        
        public static TradeController Instance
        {
            get;
            private set;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            ApproveButton.ApproveClicked += ApproveButton_ApproveClicked;
            RejectButton.RejectClicked += RejectButton_RejectClicked;
            RequestButton.RequestClicked += AddMoreButton_RequestClicked;
            
            UITradedItem.ItemSelected += UITradedItem_ItemSelected;
            UITradedItem.ItemReleased += UITradedItem_ItemReleased;
            UITradedItem.Clicked += UITradedItem_Clicked;
            
            OpponentHandController.ItemPlaced += OpponentController_ItemPlaced;
            OpponentController.Decided += OpponentController_Decided;
            
            ResultPopup.ClaimClicked += ResultPopup_ClaimClicked;
        }

        private void OnDisable()
        {
            ApproveButton.ApproveClicked -= ApproveButton_ApproveClicked;
            RejectButton.RejectClicked -= RejectButton_RejectClicked;
            RequestButton.RequestClicked -= AddMoreButton_RequestClicked;
            
            UITradedItem.ItemSelected -= UITradedItem_ItemSelected;
            UITradedItem.ItemReleased -= UITradedItem_ItemReleased;
            UITradedItem.Clicked -= UITradedItem_Clicked;
            
            OpponentHandController.ItemPlaced -= OpponentController_ItemPlaced;
            OpponentController.Decided -= OpponentController_Decided;
            
            ResultPopup.ClaimClicked -= ResultPopup_ClaimClicked;
        }

        private void FixedUpdate()
        {
            if (_selectedItem != null)
            {
                Vector3 expectedPos = Vector3.Lerp(_selectedItem.transform.position, _expectedPos,
                    Time.unscaledDeltaTime * _dragSpeed);
                _selectedItem.Move(expectedPos);
            }
        }

        public void MoveItem(Vector2 screenPos)
        {
            if (_selectedItem == null)
            {
                return;
            }

            if (IsFloorDetected(screenPos, out RaycastHit hit))
            {
                var settings = AssetsManager.Instance.TradedItemSettings;
                _expectedPos = hit.point.ChangeY(settings.ItemMoveYOffset)
                    .ChangeZ(hit.point.z + settings.ItemMoveZOffset);
                
                if (hit.point.z > 0)
                {
                    ReleaseItem();
                }
            }
        }
        
        private UIPointer WrapPointer(Vector2 screenPos)
        {
            return new UIPointer
            {
                ray = GameUISystem.Instance.Camera.ScreenPointToRay(screenPos)
            };
        }

        private void FreeItems()
        {
            foreach (var item in PlayerItems)
            {
                item.Free();
            }

            foreach (var item in OpponentItems)
            {
                item.Free();
            }
        }
        
        private void ClearItems()
        {
            PlayerItems.Clear();
            OpponentItems.Clear();
        }
        
        private void OnTraded()
        {
            LocalConfig.LevelIndex++;
            
            FreeItems();
            
            Traded(OpponentItems);
            
            _rotateGroup.rotation  = Quaternion.identity;
            OpponentItemGroup.position = Vector3.zero;
            _playerItemGroup.position = Vector3.zero;
            
            LastOpponentAction = DecisionType.Default;
            LastPlayerAction = default;
            
            UISystem.ShowWindow<ResultPopup>();
        }

        private void OnDenied()
        {
            Traded(PlayerItems);
            
            FreeItems();
            ClearItems();
        }
        
        private void ReleaseItem()
        {
            PlaceItem();
            //rigidbody logic
            
            _selectedItem = null;
        }

        private void PlaceItem()
        {
            PlayerItems.Add(_selectedItem);
            _selectedItem.Release(_expectedPos - _selectedItem.transform.position);

            if (PlayerItems.Count == 1)
            {
                LastPlayerAction = PlayerActionType.PlaceFirstItem;
            }
            else
            {
                LastPlayerAction = PlayerActionType.PlaceItem;
            }
            
            ItemPlaced(_selectedItem, false);
        }

        private bool IsFloorDetected(Vector2 screenPos, out RaycastHit hit)
        {
            bool isDetected = true;
            UIPointer pointer = WrapPointer(screenPos);
            
            Physics.SphereCast(pointer.ray, _sphereCastRadius, out hit, float.MaxValue, _floorLayer);
            if (hit.collider == null)
            {
                isDetected = false;
            }

            return isDetected;
        }

        private void TryApproveTrade()
        {
            if (_tradeRotateCor == null && LastPlayerAction == PlayerActionType.Accept &&
                LastOpponentAction == DecisionType.Accept)
            {
                _confettiFx.SetActive(true);
                
                _tradeRotateCor = StartCoroutine(TradeRotateCor());
            }
        }
        
        private void RejectButton_RejectClicked(bool isOpponent)
        {
            this.InvokeWithDelay(.3f, () =>
            {
                OnDenied();
            });
        }

        private void AddMoreButton_RequestClicked(bool isOpponent)
        {
            LastPlayerAction = PlayerActionType.RequestMore;
        }

        private void OpponentController_Decided(DecisionType decision)
        {
            LastOpponentAction = decision;

            TryApproveTrade();
        }

        private void ResultPopup_ClaimClicked()
        {
            ((IUser)User.Current).SetCurrency(CurrencyType.Coin, TradeControllerExtensions.GetTradedMoneyAmount());

            ClearItems();
        }

        private void ApproveButton_ApproveClicked(bool isOpponent)
        {
            if (!isOpponent)
            {
                LastPlayerAction = PlayerActionType.Accept;
            }

            TryApproveTrade();
        }

        private void UITradedItem_ItemSelected(UITradedItem item, Vector2 position)
        {
            if (IsFloorDetected(position, out RaycastHit hit))
            {
                var settings = AssetsManager.Instance.TradedItemSettings;
                _expectedPos = hit.point.ChangeY(AssetsManager.Instance.TradedItemSettings.ItemMoveYOffset)
                    .ChangeZ(hit.point.z + settings.ItemMoveZOffset);

                _selectedItem =
                    PoolManager.Instance.Get<TradedItem>(item.ItemPreset.Item, _expectedPos, Quaternion.identity,
                        _playerItemGroup);
            }
        }

        private void UITradedItem_ItemReleased(UITradedItem item, Vector2 position)
        {
            if (_selectedItem == null)
            {
                return;
            }
            
            if (IsFloorDetected(position, out RaycastHit hit))
            {
                bool isBoardDetected = hit.collider.TryGetComponent(out PlayableBoard board);
                
                if (isBoardDetected)
                {
                    ReleaseItem();
                }
                else
                {
                    PlaceFailed();
                    _selectedItem.Free();
                    _selectedItem = null;
                }
            }
        }

        private void UITradedItem_Clicked(UITradedItem item)
        {
            _selectedItem = PoolManager.Instance.Get<TradedItem>(item.ItemPreset.Item, _playerClickSpawnPoint.position,
                Quaternion.identity,
                _playerItemGroup);

            ReleaseItem();
        }
        
        private void OpponentController_ItemPlaced(TradedItem item)
        {
            OpponentItems.Add(item);
            item.Release(Vector3.zero);

            ItemPlaced(item, true);
        }

        private IEnumerator TradeRotateCor()
        {
            float effectTime = .4f;
            float time = 0f;
            float progress = 0f;
            var startRotation = _rotateGroup.rotation;
            var finalRotation = Quaternion.Euler(0, 180, 0);

            TradeStarted();
            
            yield return new WaitForSeconds(1f);
            
            while (time < effectTime)
            {
                yield return null;

                time += Time.deltaTime;
                progress = time / effectTime;
                
                _rotateGroup.rotation = Quaternion.Lerp(startRotation, finalRotation, progress);
            }

            _rotateGroup.rotation = finalRotation;
            
            BoardRotated();

            yield return new WaitForSeconds(0.725f);

            Vector3 playerStartPosition = _playerItemGroup.position;
            Vector3 opponentStartPosition = OpponentItemGroup.position;
            time = 0f;
            effectTime = 0.25f;
            
            while (time < effectTime)
            {
                yield return null;
                
                time += Time.deltaTime;
                progress = time / effectTime;

                _playerItemGroup.position =
                    Vector3.Lerp(playerStartPosition, new Vector3(playerStartPosition.x, playerStartPosition.y, 7f),
                        progress);

                OpponentItemGroup.position =
                    Vector3.Lerp(opponentStartPosition,
                        new Vector3(opponentStartPosition.x, opponentStartPosition.y, -7f),
                        progress);
            }
            
            OnTraded();

            _confettiFx.SetActive(false);
            
            _tradeRotateCor = null;
        }
    }
}