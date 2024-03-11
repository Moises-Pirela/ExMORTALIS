using Transendence.Core.Configs;
using UnityEngine;

namespace Transendence.Core
{
    public class EquipmentComponent : MonoBehaviour, IComponent
    {
        public int CurrentEquippedWeaponIndex;
        public int PrevEquippedWeaponIndex;
        [HideInInspector] public int[] EquippedWeaponEntityIds = new int[3];
        public Transform WeaponSpawnPoint;
        public Transform WeaponShootPoint;
        public LayerMask ShootLayer;

        private void Awake()
        {
            EquippedWeaponEntityIds = new int[3];
        }

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
