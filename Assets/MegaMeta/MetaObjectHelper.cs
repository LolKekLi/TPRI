using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class MetaObjectHelper : MonoBehaviour
    {
        private Dictionary<MetaObjectType, MetaObject[]> _metaObjects = new Dictionary<MetaObjectType, MetaObject[]>();

        public static MetaObjectHelper Instacne = null;

        private void Awake()
        {
            Instacne = this;
        }

        public void Add(MetaObjectType type, MetaObject[] metaObjects)
        {
            _metaObjects.Add(type, metaObjects);
        }

        public MetaObject[] GetMetaObjects(MetaObjectType type)
        {
            return _metaObjects[type];
        }
    }
}