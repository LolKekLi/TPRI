using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class UserManager : GameObjectSingleton<UserManager>
    {
        protected override void Init()
        {
            base.Init();
            
            User.Load();
            
            TradeController.ItemPlaced += TradeController_ItemPlaced;
            TradeController.Traded += TradeController_Traded;
        }

        protected override void DeInit()
        {
            base.DeInit();
            
            TradeController.ItemPlaced -= TradeController_ItemPlaced;
            TradeController.Traded -= TradeController_Traded;
        }

        public void PurchaseItem(ItemType item, Action<bool> callback = null)
        {
            var preset = AssetsManager.GetTradedPreset(item);
            var priceSettings = preset.PriceSettings;
            bool isSuccess = false;

            if (((IUser)User.Current).CanPurchase(priceSettings.CurrencyType, priceSettings.Price))
            {
                ((IUser)User.Current).PurchaseItem(preset);
                isSuccess = true;
            }
            else
            {
                Debug.LogError($"Not enough currency: need - {priceSettings.Price}, u have - {User.Current.Coins}");
                isSuccess = false;
            }
            
            callback?.Invoke(isSuccess);
        }

        private void TradeController_ItemPlaced(TradedItem item, bool isOpponent)
        {
            if (!isOpponent)
            {
                ((IUser)User.Current).SetItem(item.Type, -1);
            }
        }

        private void TradeController_Traded(List<TradedItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                ((IUser)User.Current).SetItem(items[i].Type, 1);
            }
        }
    }
}