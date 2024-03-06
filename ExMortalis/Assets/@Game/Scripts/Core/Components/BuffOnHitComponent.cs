using Transendence.Core.Configs;
using UnityEngine;

namespace Transendence.Core
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
