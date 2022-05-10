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
        private TextMeshProUGUI _costText = null;

        [SerializeField]
        private TextMeshProUGUI _lable = null;

        [SerializeField]
        private Button _button = null;

        private MetaObject[] _metaObjects = null;

        private Action _onClickAction = null;
        private int _cost = 0;

        private void Start()
        {
            _button.onClick.AddListener(UpMeta);
        }

        public void Refresh()
        {
            var metaLvl = LocalConfig.GetMetaLvl(_type);
            var currentObject = _metaObjects.FirstOrDefault(x => x.Level == metaLvl + 1);

            if (currentObject == null)
            {
                _costText.text = "MAX";
                _cost = 0;
            }
            else
            {
                _cost = currentObject.Cost;
                _costText.text = _cost.ToString();
                
               
            }
        }

        public void Setup(Action refresh)
        {
            _onClickAction = refresh;

            _lable.text = _type.ToString();

            _metaObjects = MetaObjectHelper.Instacne.GetMetaObjects(_type);
         }

        private void UpMeta()
        {
            if (User.Current.Coins > _cost)
            {
                ((IUser)User.Current).SetCurrency(CurrencyType.Coin, -_cost);
                LocalConfig.SetMetaLvl(_type, LocalConfig.GetMetaLvl(_type) + 1);
                
                MetaUpped(_type);

                _onClickAction.Invoke();
            }
        }
    }
}