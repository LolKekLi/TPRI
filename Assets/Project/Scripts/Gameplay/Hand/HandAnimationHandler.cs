using UnityEngine;

namespace Project
{
    public class HandAnimationHandler : MonoBehaviour
    {
        [SerializeField]
        private OpponentHandController _handController = null;

        public void PlaceItem()
        {
            _handController.PlaceItem();
        }
    }
}