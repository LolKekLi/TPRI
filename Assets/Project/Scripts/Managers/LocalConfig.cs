using UnityEngine;

namespace Project
{
    public static class LocalConfig
    {
        private class Keys
        {
            public const string LevelIndex = "LevelIndex";
            public const string BasicTutorialNeeded = "BasicTutorialNeeded";
            public const string IsInitialized = "IsInitialized";
            public const string TradeCount = "TradeCount";
            public const string StoredItem = "StoredItem_{0}";
            public const string Currency = "{0}_Currency";
            public const string MetaLvl = "MetaLvl{0}";
        }

        public static int LevelIndex
        {
            get { return PlayerPrefs.GetInt(Keys.LevelIndex, 0); }
            set { PlayerPrefs.SetInt(Keys.LevelIndex, value); }
        }

        public static void SetMetaLvl(MetaObjectType type, int lvl)
        {
            PlayerPrefs.SetInt(string.Format(Keys.MetaLvl, type), lvl);
        }

        public static int GetMetaLvl(MetaObjectType type)
        {
            return PlayerPrefs.GetInt(string.Format(Keys.MetaLvl, type), 0);
        }

        public static bool BasicTutorialNeeded
        {
            get { return false;}
            set { SetBoolValue(Keys.BasicTutorialNeeded, value); }
        }

        public static int TradeCount
        {
            get { return PlayerPrefs.GetInt(Keys.TradeCount, 0); }
            set { PlayerPrefs.SetInt(Keys.TradeCount, value); }
        }

        public static bool IsInitialized
        {
            get { return GetBoolValue(Keys.IsInitialized, false); }
            set { SetBoolValue(Keys.IsInitialized, value); }
        }

        public static int GetItemCount(ItemType type)
        {
            return PlayerPrefs.GetInt(string.Format(Keys.StoredItem, type.ToString(), 0));
        }

        public static void SetItem(ItemType type, int count)
        {
            PlayerPrefs.SetInt(string.Format(Keys.StoredItem, type.ToString()), count);
        }

        public static void SetCurrency(CurrencyType currencyType, int value)
        {
            PlayerPrefs.SetInt(string.Format(Keys.Currency, currencyType.ToString()), value);
        }

        public static int GetCurrency(CurrencyType currencyType)
        {
            return PlayerPrefs.GetInt(string.Format(Keys.Currency, currencyType.ToString()), 1000);
        }

        private static void SetBoolValue(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        private static bool GetBoolValue(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1 ? true : false;
        }
    }
}