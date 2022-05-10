using System.Linq;
using UnityEngine;

namespace Project
{
    public static class TradeControllerExtensions
    {
        public static int GetItemsDelta()
        {
            var opponentItems = TradeController.Instance.OpponentItems;
            var playerItems = TradeController.Instance.PlayerItems;

            var opponentItemsValue = opponentItems.Sum(item => AssetsManager.GetTradedPreset(item.Type).PriceSettings.Price);
            var playerItemsValue = playerItems.Sum(item => AssetsManager.GetTradedPreset(item.Type).PriceSettings.Price);
            var deltaValue = opponentItemsValue - playerItemsValue;
            
            return deltaValue;
        }

        public static int GetTradedMoneyAmount()
        {
            return Mathf.Max(AssetsManager.Instance.UserSettings.MinTradeCoinAmount,
                GetItemsDelta());
        }

        public static float GetTradeRate()
        {
            var opponentItems = TradeController.Instance.OpponentItems;
            var playerItems = TradeController.Instance.PlayerItems;

            var opponentItemsValue = opponentItems.Sum(item => item.Value);
            var playerItemsValue = playerItems.Sum(item => item.Value);
            var ratePercent = opponentItemsValue / playerItemsValue;

            return ratePercent;
        }
    }
}