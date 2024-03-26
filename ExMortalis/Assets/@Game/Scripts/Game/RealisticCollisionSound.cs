using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.ExMORTALIS
{
    [RequireComponent(typeof(AudioSource))]
    public class RealisticCollisionSound : MonoBehaviour
    {
        public AudioClip[] soundClips;
        public float minPitch = 0.8f;
        public float maxPitch = 1.2f;
        public float minVolume = 0.5f;
        public float maxVolume = 0.8f;

        private AudioSource audioSource;
        private Rigidbody rb;
        private bool canPlaySounds = false;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody>();
            StartCoroutine(EnableCollisionSoundsAfterDelay(1f));
        }

        IEnumerator EnableCollisionSoundsAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            canPlaySounds = true;
        }


        void OnCollisionEnter(Collision collision)
        {
            if (!canPlaySounds) return;

            if (collision.gameObject.transform.root == transform.root)
            {
                if (collision.collider.name == gameObject.name)
                {
                    return;
                }
            }

            AudioClip clip = soundClips[Random.Range(0, soundClips.Length)];

            foreach (ContactPoint contact in collision.contacts)
            {
                float velocityPitch = Mathf.Clamp(collision.relativeVelocity.magnitude / 10, minPitch, maxPitch);

                float directionVolume = Mathf.Clamp(Vector3.Dot(collision.contacts[0].normal, transform.forward), minVolume, maxVolume);

                float randomPitch = Random.Range(velocityPitch - 0.1f, velocityPitch + 0.1f);
                float randomVolume = Random.Range(directionVolume - 0.1f, directionVolume + 0.1f);

                audioSource.PlayOneShot(clip, randomVolume);
                audioSource.pitch = randomPitch;
            }
        }
    }

}
