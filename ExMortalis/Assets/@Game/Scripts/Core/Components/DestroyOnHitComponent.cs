using Transendence.Core.Configs;
using UnityEngine;

namespace Transendence.Core
{
    public class DestroyOnHitComponent : MonoBehaviour, IComponent
    {
        public ComponentType GetComponentType()
        {
            return ComponentType.DestroyOnHit;
        }
    }

    public class InventoryComponent : MonoBehaviour, IComponent
    {
        public int InventorySize;
        public ComponentType GetComponentType()
        {
            return ComponentType.Inventory;
        }
    }

    public class EquipmentComponent : MonoBehaviour, IComponent
    {
        public WeaponConfig[] EquippedWeapons = new WeaponConfig[3];

        public ComponentType GetComponentType()
        {
            return ComponentType.Equipment;
        }
    }
}
