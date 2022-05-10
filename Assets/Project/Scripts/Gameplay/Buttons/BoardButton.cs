using System;
using Project.UI;
using UnityEngine;

namespace Project
{
    public abstract class BoardButton : MonoBehaviour
    {
        public static event Action Clicked = delegate { };
        
        [SerializeField]
        private SelfSingleTweenController _clickController = null;

        [SerializeField]
        private GameObject _buttonFx = null;

        protected abstract bool IsOpponent
        {
            get;
        }

        private void Start()
        {
            _buttonFx.SetActive(false);
        }

        protected void Click()
        {
            OnClick();
            
            this.InvokeWithDelay(.25f, () =>
            {
                Clicked();
                
                _clickController.Play();
                
                _buttonFx.SetActive(true);
                this.InvokeWithDelay(ParticleUtility.CalculateMaxLifetime(_buttonFx), () =>
                {
                    _buttonFx.SetActive(false);
                });
            });
        }
        
        protected abstract void OnClick();
    }
}