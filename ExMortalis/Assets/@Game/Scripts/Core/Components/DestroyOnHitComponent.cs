using UnityEngine;

namespace NL.Core
{
    public class DestroyOnHitComponent : MonoBehaviour, IComponent
    {
        public ComponentType GetComponentType()
        {
            return ComponentType.DestroyOnHit;
        }
    }
}
