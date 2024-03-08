using UnityEngine;

namespace Transendence.Core
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioComponent : MonoBehaviour, IComponent
    {
        private AudioSource AudioSource;

        public void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
        }

        public void SetAudioClip(AudioClip clip)
        {
            AudioSource.clip = clip;
        }

        public void Play()
        {
            AudioSource.Play();
        }

        public void PlayOneShot(AudioClip clip)
        {
            AudioSource.PlayOneShot(clip);
        }

        public ComponentType GetComponentType()
        {
            return ComponentType.Audio;
        }
    }
}
