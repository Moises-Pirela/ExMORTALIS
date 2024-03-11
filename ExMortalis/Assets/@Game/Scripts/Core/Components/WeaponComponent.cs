using Transendence.Core.Configs;
using UnityEngine;

namespace Transendence.Core
{
    public class WeaponComponent : MonoBehaviour, IComponent
    {
        public int WielderEntityId;
        public WeaponConfig WeaponConfig;
        public AmmoCount AmmoCount;

        public ComponentType GetComponentType()
        {
            return ComponentType.Weapon;
        }
    }
}
