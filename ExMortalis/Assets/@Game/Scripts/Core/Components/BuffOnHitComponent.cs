using NL.Core.Configs;
using UnityEngine;

namespace NL.Core
{
    public class BuffOnHitComponent : MonoBehaviour, IComponent
    {
        public BuffDebuffConfig BuffDebuffConfig;

        public ComponentType GetComponentType()
        {
            return ComponentType.BuffOnHit;
        }
    }
}
