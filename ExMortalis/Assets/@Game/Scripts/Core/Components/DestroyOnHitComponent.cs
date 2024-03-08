using UnityEngine;

namespace Transendence.Core
{
    public class DestroyOnHitComponent : MonoBehaviour, IComponent
    {
        public ComponentType GetComponentType()
        {
            return ComponentType.DestroyOnHit;
        }
    }
}
