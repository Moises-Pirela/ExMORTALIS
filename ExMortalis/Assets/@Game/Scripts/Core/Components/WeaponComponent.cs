using Transendence.Core.Configs;
using Transendence.Core.Postprocess;
using UnityEngine;

namespace Transendence.Core
{
    public class WeaponComponent : MonoBehaviour, IComponent
    {
        public int WielderEntityId;
        public float NextReloadTime;
        public WeaponConfig WeaponConfig;
        public AmmoCount AmmoCount;

        public Animator WeaponEntityAnimator;
        public Animator ParentEntityAnimator;

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

        public void SetAnimation(string weaponAnimation, string parentAnimation)
        {
            WeaponEntityAnimator.SetTrigger(weaponAnimation);
            //ParentEntityAnimator.SetTrigger(parentAnimation);
        }

        public void SendReloadEvent()
        {
            ReloadWeaponPostProcessEvent reloadEvent = new ReloadWeaponPostProcessEvent();

            reloadEvent.WeaponHolderEntityId = World.PLAYER_ENTITY_ID;

            World.Instance.AddPostProcessEvent(reloadEvent);
        }

        public ComponentType GetComponentType()
        {
            return ComponentType.Weapon;
        }
    }
}
