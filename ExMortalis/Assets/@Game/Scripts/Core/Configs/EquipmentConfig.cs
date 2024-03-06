using UnityEngine;

namespace Transendence.Core.Configs
{
    public enum EquipmentType { Consumable, Weapon }
    [CreateAssetMenu(menuName = "Tools/Configs/Equipment", fileName = "Equipment")]
    public class EquipmentConfig : InventoryItemConfig
    {
        public EquipmentType EquipmentType;
    }
}