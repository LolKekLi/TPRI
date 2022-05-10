using UnityEngine;

namespace Project
{
    public class ParticlesManager : GameObjectSingleton<ParticlesManager>
    {
        private PooledParticle _placeFx = null;
        
        protected override void Init()
        {
            base.Init();

            _placeFx = PoolManager.Instance.Get<PooledParticle>(AssetsManager.Instance.FxSettings.PlaceItemFx, Vector3.zero, Quaternion.identity);
            
            TradedItem.Placed += TradedItem_Placed;
        }

        protected override void DeInit()
        {
            base.DeInit();
            
            TradedItem.Placed -= TradedItem_Placed;
        }

        private void TradedItem_Placed(TradedItem item)
        {
            _placeFx.Emit(item.transform.position);
        }
    }
}