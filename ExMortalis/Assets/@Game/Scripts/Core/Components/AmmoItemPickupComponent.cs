using Transendence.Core.Configs;
using UnityEngine;

namespace Transendence.Core
{
    public class AmmoItemPickupComponent : MonoBehaviour, IComponent
    {
        public int AmmoCount;
        public AmmoType AmmoType;
        
        public ComponentType GetComponentType()
        {
            return ComponentType.AmmoItemPickup;
        }
    }
}
