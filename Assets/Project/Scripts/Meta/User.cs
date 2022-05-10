using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class User : IUser
    {
        public static Action<ItemType> Changed = delegate { };
        public static Action CurrencyChanged = delegate { };
        public static Action Purchased = delegate { };
        
        private Dictionary<ItemType, int> _items = new Dictionary<ItemType, int>();

        public int Coins
        {
            get
            {
                return LocalConfig.GetCurrency(CurrencyType.Coin);
            }

            private set
            {
                LocalConfig.SetCurrency(CurrencyType.Coin, value);
                CurrencyChanged();
            }
        }
        
        public static User Current
        {
            get;
            private set;
        }

        public static void Load()
        {
            Current = new User();

            Current.LoadData();
        }

        private void InitializeUser()
        {
            var userSettings = AssetsManager.Instance.UserSettings;
            
            foreach (var itemPreset in userSettings.ItemPresets)
            {
                ((IUser)Current).SetItem(itemPreset.Type, itemPreset.Count);
            }

            Coins = userSettings.InitialGold;

            LocalConfig.IsInitialized = true;
        }
        
        private void LoadData()
        {
            var items = (ItemType[])Enum.GetValues(typeof(ItemType));

            for (int i = 0; i < items.Length; i++)
            {
                _items.Add(items[i], LocalConfig.GetItemCount(items[i]));
            }
            
            if (!LocalConfig.IsInitialized)
            {
                InitializeUser();
            }

            CurrencyChanged();
        }

        private void SaveItem(ItemType type)
        {
            LocalConfig.SetItem(type, _items[type]);

            Changed(type);
        }

        void IUser.PurchaseItem(TradedItemPreset item)
        {
            Purchased();
            
            Coins -= item.PriceSettings.Price;
            _items[item.ItemType] += item.PriceSettings.RewardCount;

            SaveItem(item.ItemType);
        }

        bool IUser.CanPurchase(CurrencyType type, int amount)
        {
            bool canPurchase = false;
            
            switch (type)
            {
                case CurrencyType.Coin:
                    canPurchase = amount <= Current.Coins;
                    break;
                
                default:
                    Debug.LogException(new Exception($"Not implemented case for {nameof(CurrencyType)}: {type}"));
                    break;
            }

            return canPurchase;
        }

        void IUser.SetItem(ItemType itemType, int count)
        {
            _items[itemType] += count;
            
            SaveItem(itemType);
        }

        void IUser.SetCurrency(CurrencyType type, int amount)
        {
            if (type == CurrencyType.Coin)
            {
                Coins += amount;
            }
        }

        public int GetTradeItemCount(ItemType type)
        {
            return _items[type];
        }

        public int GetTotalAmount()
        {
            int amount = 0;
            var items = (ItemType[])Enum.GetValues(typeof(ItemType));
            
            for (int i = 0; i < items.Length; i++)
            {
                amount += GetTradeItemCount(items[i]);
            }

            return amount;
        }
    }
}