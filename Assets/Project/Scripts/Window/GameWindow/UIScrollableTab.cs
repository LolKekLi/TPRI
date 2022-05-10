using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class UIScrollableTab : MonoBehaviour
    {
        private const int PrepareItemCount = 10;

        [SerializeField]
        private UITradedItem _tradedItemPrefab = null;

        [SerializeField]
        private Transform _itemParent = null;

        [SerializeField]
        private ScrollRect _scrollRect = null;

        private List<UITradedItem> _tradedItems = new List<UITradedItem>();
        
        public void Prepare()
        {
            UITradedItem item = null;
            
            for (int i = 0; i < PrepareItemCount; i++)
            {
                item = Instantiate(_tradedItemPrefab, Vector3.zero, Quaternion.identity, _itemParent);
                _tradedItems.Add(item);
            }
        }
        
        public void Setup(CategoryType category)
        {
            var items = CategoryHelper.GetItemsByCategory(category);

            int index = 0;
            for (; index < items.Length; index++)
            {
                _tradedItems[index].Setup(_scrollRect, AssetsManager.GetTradedPreset(items[index]));
            }

            for (; index < _tradedItems.Count; index++)
            {
                _tradedItems[index].Hide();
            }
        }

        public UITradedItem GetFirstItem()
        {
            return _tradedItems[0];
        }
    }
}