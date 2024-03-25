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

        public ComponentType GetComponentType()
        {
            return ComponentType.Throwable;
        }
    }
}
