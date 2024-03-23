using UnityEngine;

namespace NL.Core
{
    public class ProjectileComponent : MonoBehaviour, IComponent
    {
        public float MaxDistance;
        public ComponentType GetComponentType() => ComponentType.Projectile;
    }
}
