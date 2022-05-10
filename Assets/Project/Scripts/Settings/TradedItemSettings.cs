using System;
using System.Linq;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class PriceSettings
    {
        [field: SerializeField]
        public CurrencyType CurrencyType
        {
            get;
            private set;
        }

        [field: SerializeField]
        public int Price
        {
            get;
            private set;
        }

        [field: SerializeField]
        public int RewardCount
        {
            get;
            private set;
        }
    }
    
    [Serializable]
    public class TradedItemPreset
    {
        [field: SerializeField]
        public CategoryType CategoryType
        {
            get;
            private set;
        }

        [field: SerializeField]
        public RarityType RarityType
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public TradedItem Item
        {
            get;
            private set;
        }

        [field: SerializeField]
        public Sprite ItemSprite
        {
            get;
            private set;
        }

        [field: SerializeField]
        public int ItemValue
        {
            get;
            private set;
        }

        [field: SerializeField]
        public PriceSettings PriceSettings
        {
            get;
            private set;
        }

        public ItemType ItemType
        {
            get => Item.Type;
        }
        
        public float GetValue()
        {
            return ItemValue;
        }
    }
    
    [CreateAssetMenu(fileName = "TradedItemSettings", menuName = "Scriptable/TradedItemSettings", order = 0)]
    public class TradedItemSettings : ScriptableObject
    {
        [SerializeField]
        private TradedItemPreset[] _presets = null;

        [field: SerializeField]
        public float ForceMultiplier
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public float ItemMoveYOffset
        {
            get;
            private set;
        }

        [field: SerializeField]
        public float ItemMoveZOffset
        {
            get;
            private set;
        }
        
        public TradedItemPreset GetPreset(ItemType type)
        {
            TradedItemPreset preset = _presets.FirstOrDefault(p => p.ItemType == type);

            if (preset == null)
            {
#if UNITY_EDITOR
                Debug.LogException(new Exception(
                    $"[{nameof(TradedItemSettings)}] GetPreset: not found {nameof(TradedItemPreset)} for type - {type}"));
#endif
            }

            return preset;
        }
    }
}