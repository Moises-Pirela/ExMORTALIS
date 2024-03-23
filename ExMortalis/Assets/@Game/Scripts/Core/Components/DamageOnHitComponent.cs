using UnityEngine;

namespace NL.Core
{
    public class DamageOnHitComponent : MonoBehaviour, IComponent
    {
        public ComponentType GetComponentType()
        {
            return ComponentType.DamageOnHit;
        }
    }
}
