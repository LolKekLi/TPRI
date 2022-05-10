using UnityEngine;

namespace Project
{
    public class PlayableBoard : MonoBehaviour
    {
        public static PlayableBoard Instance
        {
            get;
            private set;
        }

        public Rigidbody Rigidbody
        {
            get;
            private set;
        }

        private void Awake()
        {
            Instance = this;

            Rigidbody = GetComponent<Rigidbody>();
        }
    }
}