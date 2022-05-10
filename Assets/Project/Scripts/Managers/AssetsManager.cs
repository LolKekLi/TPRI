using UnityEngine;

namespace Project
{
    public partial class AssetsManager : GameObjectSingleton<AssetsManager>
    {
        [field: SerializeField]
        public TradedItemSettings TradedItemSettings
        {
            get;
            private set;
        }

        [field: SerializeField]
        public RaritySettings RaritySettings
        {
            get;
            private set;
        }

        [field: SerializeField]
        public OpponentSettings OpponentSettings
        {
            get;
            private set;
        }

        [field: SerializeField]
        public CategorySettings CategorySettings
        {
            get;
            private set;
        }

        [field: SerializeField]
        public UserSettings UserSettings
        {
            get;
            private set;
        }

        [field: SerializeField]
        public FxSettings FxSettings
        {
            get;
            private set;
        }
        
        public static TradedItemPreset GetTradedPreset(ItemType type)
        {
            return Instance.TradedItemSettings.GetPreset(type);
        }
    }
}