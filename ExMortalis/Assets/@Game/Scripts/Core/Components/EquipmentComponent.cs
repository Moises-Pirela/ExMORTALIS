using Transendence.Core.Configs;
using UnityEngine;

namespace Transendence.Core
{
    public class EquipmentComponent : MonoBehaviour, IComponent
    {
        public int CurrentEquippedWeaponIndex;
        public int PrevEquippedWeaponIndex;
        [HideInInspector] public int[] EquippedItemEntityIds = new int[3];
        public Transform WeaponSpawnPoint;
        public Transform WeaponShootPoint;
        public LayerMask ShootLayer;

        private void Awake()
        {
            EquippedItemEntityIds = new int[3] { -1, -1, -1 };
            CurrentEquippedWeaponIndex = -1;
        }

        public ComponentType GetComponentType()
        {
            return ComponentType.Equipment;
        }

        public void UpdateWeaponEquipped(int weaponIndex)
        {
            PrevEquippedWeaponIndex = CurrentEquippedWeaponIndex;
            CurrentEquippedWeaponIndex = weaponIndex;
        }
    }

    public struct AmmoCount
    {
        public int CurrentCount;
        public int MaxCount;
        public DamageType AmmoType;
    }
}
