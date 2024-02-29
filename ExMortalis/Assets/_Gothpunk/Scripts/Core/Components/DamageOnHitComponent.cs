using UnityEngine;

namespace Transendence.Core
{
    public class DamageOnHitComponent : MonoBehaviour, IComponent
    {
        public ComponentType GetComponentType()
        {
            return ComponentType.DamageOnHit;
        }
    }
}
