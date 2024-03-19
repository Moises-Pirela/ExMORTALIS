using Transendence.Core.Configs;
using UnityEngine;

namespace Transendence.Core
{
    public class WeaponPickUpComponent : MonoBehaviour, IComponent
    {
        public WeaponConfig WeaponConfig;

        public ComponentType GetComponentType()
        {
            return ComponentType.WeaponPickUp;
        }
    }
}
