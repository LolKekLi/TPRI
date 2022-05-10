using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "ParticleSettings", menuName = "Scriptable/ParticleSettings", order = 0)]
    public class FxSettings : ScriptableObject
    {
        [field: SerializeField]
        public PooledParticle PlaceItemFx
        {
            get;
            private set;
        }
    }
}