using System;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class InitialItemPreset
    {
        [field: SerializeField]
        public ItemType Type
        {
            get;
            private set;
        }

        [field: SerializeField]
        public int Count
        {
            get;
            private set;
        }
    }
    
    [CreateAssetMenu(fileName = "UserSettings", menuName = "Scriptable/UserSettings", order = 0)]
    public class UserSettings : ScriptableObject
    {
        [field: SerializeField]
        public InitialItemPreset[] ItemPresets
        {
            get;
            private set;
        }

        [field: SerializeField]
        public int InitialGold
        {
            get;
            private set;
        }

        [field: SerializeField]
        public int MinTradeCoinAmount
        {
            get;
            private set;
        }
    }
}