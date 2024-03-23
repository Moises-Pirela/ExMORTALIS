using UnityEngine;

namespace NL.Core
{
    public class MovementComponent : MonoBehaviour, IComponent
    {
        public Vector3 MovementVector;
        public float MovementSpeed;

        public ComponentType GetComponentType()
        {
            return ComponentType.Movement;
        }

        public void Tick()
        {
            Vector3 targetPosition = transform.position + MovementVector * MovementSpeed * Time.deltaTime;

            transform.position = targetPosition;
        }
    }
}


