using System;
using System.Linq;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class CategoryPreset
    {
        [field: SerializeField]
        public CategoryType Type
        {
            get;
            private set;
        }

        [field: SerializeField]
        public Sprite CategoryIcon
        {
            get;
            private set;
        }
    }
    
    [CreateAssetMenu(fileName = "CategorySettings", menuName = "Scriptable/CategorySettings", order = 0)]
    public class CategorySettings : ScriptableObject
    {
        [SerializeField]
        private CategoryPreset[] _categoryPresets = null;

        public CategoryPreset GetCategoryPreset(CategoryType type)
        {
            var preset = _categoryPresets.FirstOrDefault(category => category.Type == type);

            if (preset == null)
            {
                Debug.LogException(new Exception($"Not found {nameof(CategoryPreset)} for type: {type}"));
            }
            
            return preset;
        }
    }
}