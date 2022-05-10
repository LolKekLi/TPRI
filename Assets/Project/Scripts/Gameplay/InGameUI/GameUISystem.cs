using UnityEngine;

namespace Project
{
    public class GameUISystem : MonoBehaviour
    {
        private const float YOffset = -3f;
        private const float XOffset = -.25f;
        
        [SerializeField]
        private IngmaeUIEmoji _acceptGroup = null;

        [SerializeField]
        private Transform _acceptButton = null;

        [SerializeField]
        private IngmaeUIEmoji _requestMoreGroup = null;

        [SerializeField]
        private Transform _requestMoreButton = null;
        
        [field: SerializeField]
        public Camera Camera
        {
            get;
            private set;
        }
        
        public static GameUISystem Instance
        {
            get;
            private set;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            var position = _acceptButton.transform.position;
            _acceptGroup.Setup(GetViewportPosition(_acceptButton.position.ChangeY(position.y + YOffset).ChangeX(position.x + XOffset)));
            _requestMoreGroup.Setup(GetViewportPosition(_requestMoreButton.position.ChangeY(_requestMoreButton.transform.position.y + YOffset)));
        }

        private void OnEnable()
        {
            OpponentController.Decided += OpponentController_Decided;
        }

        private void OnDisable()
        {
            OpponentController.Decided -= OpponentController_Decided;
        }
        
        private void OpponentController_Decided(DecisionType decision)
        {
            if (decision == DecisionType.Accept)
            {
                _acceptGroup.Show();
            }
            else if (decision == DecisionType.Deny || decision == DecisionType.RequestMore)
            {
                _requestMoreGroup.Show();
            }
        }

        public Vector2 GetViewportPosition(Vector3 worldPosition)
        {
            return Camera.WorldToViewportPoint(worldPosition);
        }
    }
}