using UnityEngine;

namespace Project
{
    public class MetaObject : MonoBehaviour
    {
        [field: SerializeField]
        public MetaObjectType Type
        {
            get;
            private set;
        }

        [field: SerializeField]
        public int Level
        {
            get;
            private set;
        }

        [field: SerializeField]
        public int Cost
        {
            get;
            private set;
        }
    }
}
