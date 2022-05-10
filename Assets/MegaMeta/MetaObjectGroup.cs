using System.Linq;
using UnityEngine;

namespace Project
{
    public class MetaObjectGroup : MonoBehaviour
    {
        private void Start()
        {
            var metaObject = GetComponentsInChildren<MetaObject>();

            for (int i = 0; i < metaObject.Length; i++)
            {
                metaObject[i].gameObject.SetActive(false);
            }

            var metaLvl = LocalConfig.GetMetaLvl(metaObject[0].Type);

            metaObject.FirstOrDefault(x => x.Level == metaLvl)?.gameObject.SetActive(true);
            
            MetaObjectHelper.Instacne.Add(metaObject[0].Type, metaObject);
        }
    }
}