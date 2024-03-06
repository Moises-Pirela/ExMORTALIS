using UnityEngine;

namespace Transendence.Core
{
    public class ThrowableComponent : MonoBehaviour, IComponent
    {
        public float ThrowForce;
        [HideInInspector] public Vector3 ThrowDirection;
        [HideInInspector] public Rigidbody Rigidbody;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void SetThrow()
        {
            Rigidbody.velocity = ThrowDirection * ThrowForce;
        }

        public ComponentType GetComponentType()
        {
            return ComponentType.Throwable;
        }
    }
}
