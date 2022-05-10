using System;
using BoingKit;
using UnityEngine;
using Collision = UnityEngine.Collision;

namespace Project
{
    [RequireComponent(typeof(Rigidbody))]
    public class TradedItem : PooledBehaviour
    {
        public static event Action<TradedItem> Placed = delegate { };

        [SerializeField]
        private BoingBones _boingBones = null;
        
        private bool _isPlaced = false;
        private TradedItemPreset _settings = null;
        
        [field: SerializeField]
        public ItemType Type
        {
            get;
            private set;
        }
        
        public Rigidbody Rigidbody
        {
            get;
            private set;
        }

        public float Value
        {
            get => _settings.GetValue();
        }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!_isPlaced && !Rigidbody.isKinematic && (other.gameObject.TryGetComponent(out PlayableBoard board) ||
                                                          other.gameObject.TryGetComponent(out TradedItem item)))
            {
                OnPlaced();
            }
        }

        public override void Init()
        {
            base.Init();

            _settings = AssetsManager.GetTradedPreset(Type);
        }

        public override void SpawnFromPool()
        {
            base.SpawnFromPool();

            if (_boingBones)
            {
                _boingBones.enabled = true;
            }

            _isPlaced = false;
            Rigidbody.isKinematic = true;
        }

        protected virtual void OnPlaced()
        {
            if (_boingBones)
            {
                _boingBones.enabled = false;
            }

            _isPlaced = true;
            Placed(this);
        }

        public void Release(Vector3 delta)
        {
            Rigidbody.isKinematic = false;
            
            Rigidbody.AddForce(delta * AssetsManager.Instance.TradedItemSettings.ForceMultiplier);
        }

        public void Move(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }
    }
}