using Transendence.Core.Configs;
using UnityEngine;

namespace Transendence.Core
{
    public class WeaponComponent : MonoBehaviour, IComponent
    {
        public int WielderEntityId;
        public WeaponConfig WeaponConfig;
        public AmmoCount AmmoCount;
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
            return ComponentType.Weapon;
        }


    }
}
