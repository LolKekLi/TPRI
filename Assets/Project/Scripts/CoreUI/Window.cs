using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public abstract class Window : MonoBehaviour
    {
        public abstract bool IsPopup
        {
            get;
        }

        protected virtual void Start()
        {

        }

        protected virtual void OnEnable()
        {
            User.CurrencyChanged += User_CurrencyChanged;
        }

        protected virtual void OnDisable()
        {
            User.CurrencyChanged -= User_CurrencyChanged;
        }

        public virtual void Preload()
        {
            gameObject.SetActive(false);
        }

        public virtual void OnShow()
        {
            gameObject.SetActive(true);
        }

        public virtual void Refresh()
        {

        }

        public virtual void OnHide()
        {
            gameObject.SetActive(false);
        }

        public T GetDataValue<T>(string itemKey, T defaultValue = default, Dictionary<string, object> forcedData = null)
        {
            Dictionary<string, object> data = UISystem.Data;

            if (data == null || data.Count == 0)
            {
                return defaultValue;
            }

            if (!data.TryGetValue(itemKey, out object itemObject))
            {
                return defaultValue;
            }

            if (itemObject is T)
            {
                return (T)itemObject;
            }

            return defaultValue;
        }

        protected void SetDataValue<T>(string itemKey, T value)
        {
            if (!UISystem.Data.ContainsKey(itemKey))
            {
                UISystem.Data.Add(itemKey, value);
            }
            else
            {
                UISystem.Data[itemKey] = value;
            }
        }

        private void User_CurrencyChanged()
        {
            Refresh();
        }
    }
}