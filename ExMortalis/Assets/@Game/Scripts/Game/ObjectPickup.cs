using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.ExMORTALIS
{
    public class ObjectPickup : MonoBehaviour
    {
        public Transform grabPoint; // The point from which the object is grabbed
        public float swayAmplitude = 0.1f; // Amplitude of the sway effect
        public float swaySpeed = 1f; // Speed of the sway effect
        private GameObject pickedObject;
        private float swayOffset;

        void Update()
        {
            if ( UnityEngine.Input.GetButtonDown("Fire2")) // Assuming "Fire1" is your pick-up button
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Throwable"))
                    {
                        pickedObject = hit.collider.gameObject;
                        pickedObject.transform.SetParent(grabPoint);
                        // Ensure the object's Rigidbody is not set to kinematic
                        pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                    }
                }
            }

            if (UnityEngine.Input.GetButtonUp("Fire2") && pickedObject != null)
            {
                pickedObject.transform.SetParent(null);
                pickedObject.GetComponent<Rigidbody>().isKinematic = true; // Set to kinematic when dropped
                pickedObject = null;
            }

            // Apply sway effect
            if (pickedObject != null)
            {
                swayOffset += Time.deltaTime * swaySpeed;
                float sway = Mathf.Sin(swayOffset) * swayAmplitude;
                pickedObject.transform.localPosition = new Vector3(0, sway, 0);
            }
        }

        void FixedUpdate()
        {
            // Update the object's position and rotation to match the grab point
            if (pickedObject != null)
            {
                pickedObject.transform.position = grabPoint.position;
                pickedObject.transform.rotation = grabPoint.rotation;
            }
        }
    }

}
