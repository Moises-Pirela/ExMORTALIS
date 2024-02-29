using System.Collections.Generic;
using UnityEngine;

namespace Transendence.Core
{
    public struct CollisionInfo
    {
        public int CollidedEntityId;
        public Vector3 ContactPoint;
        public bool HasCollided;
    }

    public class CollisionComponent : MonoBehaviour, IComponent
    {
        public Dictionary<int, CollisionInfo> Collisions = new Dictionary<int, CollisionInfo>();
        public Dictionary<int, CollisionInfo> Triggers = new Dictionary<int, CollisionInfo>();

        public ComponentType GetComponentType()
        {
            return ComponentType.Collision;
        }
    }
}
