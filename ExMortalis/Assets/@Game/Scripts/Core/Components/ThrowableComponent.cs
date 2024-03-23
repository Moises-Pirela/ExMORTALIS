using UnityEngine;

namespace NL.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class ThrowableComponent : MonoBehaviour, IComponent
    {
        [HideInInspector] public Rigidbody Rigidbody;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void Throw(float throwForce, Vector3 throwDirection)
        {
            Rigidbody.velocity = throwDirection * throwForce;
        }

        public ComponentType GetComponentType()
        {
            return ComponentType.Throwable;
        }
    }
}
