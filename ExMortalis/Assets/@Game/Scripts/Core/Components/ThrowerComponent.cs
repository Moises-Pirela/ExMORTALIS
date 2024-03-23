using UnityEngine;

namespace NL.Core
{
    public class ThrowerComponent : MonoBehaviour, IComponent
    {
        public Transform PickupTransform;

        [HideInInspector] public Vector3 ThrowDirection;
        [HideInInspector] public float ThrowDistance;

        [HideInInspector] public int PickedUpEntityId = -1;

        public ComponentType GetComponentType()
        {
            return ComponentType.Thrower;
        }
    }
}
