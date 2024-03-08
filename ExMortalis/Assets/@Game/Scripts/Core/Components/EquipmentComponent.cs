using Transendence.Core.Configs;
using UnityEngine;

namespace Transendence.Core
{
    public class EquipmentComponent : MonoBehaviour, IComponent
    {
        public int CurrentEquippedWeaponIndex;
        public int PrevEquippedWeaponIndex;
        public Weapon[] EquippedWeapons = new Weapon[3];
        public EquipmentConfig[] EquippedEquipment = new EquipmentConfig[7];
        public Transform WeaponSpawnPoint;
        public LayerMask ShootLayer;

        public ComponentType GetComponentType()
        {
            return ComponentType.Equipment;
        }

        public void UpdateEquippedWeapon()
        {
            
        }
    }

    public struct AmmoCount
    {
        public int CurrentCount;
        public int MaxCount;
        public AmmoType AmmoType;
    }
}
