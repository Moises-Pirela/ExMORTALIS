using UnityEngine;

namespace NL.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class KickableComponent : MonoBehaviour, IComponent
    {
        public AudioClip HitSound;
        public AudioSource AudioSource;

        [HideInInspector] public Rigidbody Rigidbody;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            AudioSource = GetComponent<AudioSource>();
        }

        public void PlayHitSound()
        {
            AudioSource.PlayOneShot(HitSound);
        }

        public ComponentType GetComponentType()
        {
            return ComponentType.Kickable; 
        }
    }
}
