using System;
using System.Collections.Generic;
using System.Linq;

namespace Project
{
    public static class CategoryHelper
    {
        public static ItemType[] GetItemsByCategory(CategoryType type)
        {
            List<TradedItemPreset> presets = new List<TradedItemPreset>();
            var items = (ItemType[])Enum.GetValues(typeof(ItemType));
            TradedItemPreset preset = null;

            for (int i = 0; i < items.Length; i++)
            {
                preset = AssetsManager.GetTradedPreset(items[i]);
                
                if (preset.CategoryType == type)
                {
                    presets.Add(preset);
                }
            }

            presets = presets.OrderBy(i => i.RarityType).ThenBy(i => i.PriceSettings.Price).ToList();
            
            return presets.Select(p => p.ItemType).ToArray();
        }
    }
}