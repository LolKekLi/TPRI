using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class UpButton : MonoBehaviour
    {
        public static event Action<MetaObjectType> MetaUpped = delegate { };

        [SerializeField]
        private MetaObjectType _type = default;

        [SerializeField]
        private TextMeshProUGUI _cost = null;

        [SerializeField]
        private Button _button = null;

        private MetaObject[] _metaObjects = null;

        private Action _onClickAction = null;

        private void Start()
        {
            _button.onClick.AddListener(UpMeta);
        }

        public void Refresh()
        {
            var metaLvl = LocalConfig.GetMetaLvl(_type);
            var currentObject = _metaObjects.FirstOrDefault(x => x.Level == metaLvl);

            _cost.text = currentObject.Cost.ToString();
        }

        public void Setup(Action refresh)
        {
            _onClickAction = refresh;

            _metaObjects = MetaObjectHelper.Instacne.GetMetaObjects(_type);

            Refresh();
        }

        private void UpMeta()
        {
            var cost = Int32.Parse(_cost.text);

            if (User.Current.Coins > cost)
            {
                ((IUser)User.Current).SetCurrency(CurrencyType.Coin, -cost);
                LocalConfig.SetMetaLvl(_type, LocalConfig.GetMetaLvl(_type) + 1);
                
                MetaUpped(_type);

                _onClickAction.Invoke();
            }
        }
    }
}