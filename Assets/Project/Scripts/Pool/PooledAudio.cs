using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(AudioSource))]
    public class PooledAudio : PooledBehaviour
    {
        [SerializeField]
        private AudioSource _source = null;

        public AudioSource Source
        {
            get => _source;
        }

        public void Setup(AudioClip clip)
        {
            _source.clip = clip;
            FreeTimeout = clip.length;
            _source.Play();
        }
    }
}