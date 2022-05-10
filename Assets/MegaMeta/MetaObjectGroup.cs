using System;
using System.Linq;
using Project.UI;
using UnityEngine;

namespace Project
{
    public class MetaObjectGroup : MonoBehaviour
    {
        [SerializeField]
        private MetaObject[] _metaObject = null;

        private MetaObject _currentObject = null;
        
        private void OnEnable()
        {
            UpButton.MetaUpped += UpButton_MetaUpped;
        }
        
        private void OnDisable()
        {
            UpButton.MetaUpped += UpButton_MetaUpped;
        }
        
        private void Awake()
        {
            for (int i = 0; i < _metaObject.Length; i++)
            {
                _metaObject[i].gameObject.SetActive(false);
            }

            var metaLvl = LocalConfig.GetMetaLvl(_metaObject[0].Type);
            _currentObject = _metaObject.FirstOrDefault(x => x.Level == metaLvl);

            if(_currentObject == null)
            {
                _metaObject.Max(x=> x.Level).gameObject.SetActive(true);
            }
            else
            {
                _currentObject.gameObject.SetActive(true);
            }
            
            MetaObjectHelper.Instacne.Add(_metaObject[0].Type, _metaObject);
        }
        
        private void UpButton_MetaUpped(MetaObjectType type)
        {
            var thisType = _metaObject[0].Type;
            
            if (type == thisType)
            {
                var obj = _metaObject.FirstOrDefault(x => x.Level == LocalConfig.GetMetaLvl(thisType));
                
                if (obj != null)
                {
                    _currentObject.gameObject.SetActive(false);
                    _currentObject = obj;
                    _currentObject.gameObject.SetActive(true);
                }
            }
        }
    }
}