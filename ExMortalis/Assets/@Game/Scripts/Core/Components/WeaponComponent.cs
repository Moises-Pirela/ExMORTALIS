using NL.Core.Configs;
using NL.Core.Postprocess;
using UnityEngine;

namespace NL.Core
{
    public class WeaponComponent : MonoBehaviour, IComponent
    {
        public enum WeaponState { Idle, Firing, Reloading }
        public WeaponState State;
        public int WielderEntityId;
        public float NextReloadTimeSec;
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
