using UnityEngine;

namespace Project
{
    public class TutorialManager : GameObjectSingleton<TutorialManager>
    {
        [SerializeField]
        private GameObject _handPrefab = null;

        private UITutorialHand _spawnedHand = null;

        private void StartTutorial()
        {
            // var element = GameManager.Instance.Level.TutorialFoldingElement;
            //
            // this.InvokeWithDelay(.25f, () =>
            // {
            //     var viewportPos = GameManager.Instance.Camera.WorldToViewportPoint(element.transform.position);
            //     var swipeDirection = element.SwipeDirection() * 0.075f;
            //
            //     SpawnAnimatedHand(viewportPos - swipeDirection, viewportPos + swipeDirection);
            // });
        }

        public UITutorialHand SpawnAnimatedHand(Vector2 pos, Vector2 endPos, Vector2 offset = default, float rotate = 0)
        {
            FreeHand();
            var hand = SetupHand(pos, offset, rotate);
            hand.SetupAnimatedHand(endPos);

            return hand;
        }

        public UITutorialHand SetupHand(Vector2 pos, Vector2 offset = default, float rotate = 0, Transform parent = null)
        {
            var handGO = Instantiate(_handPrefab, parent ? parent : transform);
            _spawnedHand = handGO.GetComponent<UITutorialHand>();

            if (_spawnedHand != null)
            {
                _spawnedHand.SetupAnchorPos(pos, offset, rotate);
            }

            return _spawnedHand;
        }

        public static void FreeHand()
        {
            if (Instance._spawnedHand != null)
            {
                Instance._spawnedHand.Free();
                Instance._spawnedHand = null;
            }
        }
    }
}