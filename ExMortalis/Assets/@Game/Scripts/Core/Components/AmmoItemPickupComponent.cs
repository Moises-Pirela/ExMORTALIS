using NL.Core.Configs;
using UnityEngine;

namespace NL.Core
{
    public class AmmoItemPickupComponent : MonoBehaviour, IComponent
    {
        public InventoryItemConfig ItemConfig;
        public int AmmoCount;
        public DamageType AmmoDamageType;
        public AmmoWeaponType WeaponAmmoType;
        
        public ComponentType GetComponentType()
        {
            return ComponentType.AmmoItemPickup;
        }
    }
}
