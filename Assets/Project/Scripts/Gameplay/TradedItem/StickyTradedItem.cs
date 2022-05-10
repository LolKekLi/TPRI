using UnityEngine;

namespace Project
{
    public class StickyTradedItem : TradedItem
    {
        private const float RayLength = 1f;
        
        [SerializeField]
        private Transform _stickyPoint = null;

        private FixedJoint _joint = null;

        public override void SpawnFromPool()
        {
            base.SpawnFromPool();

            if (_joint)
            {
                Destroy(_joint);
            }
        }

        protected override void OnPlaced()
        {
            base.OnPlaced();

            Ray ray = new Ray(_stickyPoint.position, -_stickyPoint.up);
            if (Physics.Raycast(ray, out RaycastHit hit, RayLength))
            {
                var item = hit.collider.gameObject.GetComponentInParent<TradedItem>();
                
                if (item != null)
                {
                    SetupJoint(hit, item.Rigidbody);
                }
                else if (hit.collider.TryGetComponent(out PlayableBoard board))
                {
                    SetupJoint(hit, board.Rigidbody);
                }
            }
        }

        private void SetupJoint(RaycastHit hit, Rigidbody rigidbody)
        {
            var t = transform;
            t.position = hit.point;
            t.up = hit.normal;
            _joint = gameObject.AddComponent<FixedJoint>();
            _joint.connectedBody = rigidbody;
        }
    }
}