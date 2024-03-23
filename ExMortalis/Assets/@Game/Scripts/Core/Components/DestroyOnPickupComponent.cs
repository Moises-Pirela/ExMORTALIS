using UnityEngine;

namespace NL.Core
{
    public class DestroyOnPickupComponent : MonoBehaviour, IComponent
    {
        public ComponentType GetComponentType()
        {
            return ComponentType.DestroyOnPickup;
        }
    }
}
