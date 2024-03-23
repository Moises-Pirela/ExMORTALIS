using NL.Core.Configs;
using UnityEngine;

namespace NL.Core
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
