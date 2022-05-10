using System;
using System.Linq;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class RarityPreset
    {
        [field: SerializeField]
        public RarityType Type
        {
            get;
            private set;
        }

        [field: SerializeField]
        public Color BackgroundColor
        {
            get;
            private set;
        }

        [field: SerializeField]
        public Color BackgroundStrokeColor
        {
            get;
            private set;
        }

        [field: SerializeField]
        public Color CounterStrokeColor
        {
            get;
            private set;
        }

        [field: SerializeField]
        public Sprite FxSprite
        {
            get;
            private set;
        }
    }
    
    [CreateAssetMenu(fileName = "RaritySettings", menuName = "Scriptable/RaritySettings", order = 0)]
    public class RaritySettings : ScriptableObject
    {
        [SerializeField]
        private RarityPreset[] _presets = null;

        public RarityPreset GetPreset(RarityType type)
        {
            var preset = _presets.FirstOrDefault(p => p.Type == type);

            if (preset == null)
            {
#if UNITY_EDITOR
                Debug.LogException(new Exception($"[{nameof(RaritySettings)}] GetRarityValue: not found {nameof(RarityPreset)} for type - {type}"));
#endif
            }
            
            return preset;
        }
    }
}