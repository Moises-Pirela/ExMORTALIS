using UnityEngine;
using Unity.Mathematics;

namespace Transendence.Core.Configs
{
    public enum ItemType
    {
        Junk,
        Equippable
    }

    [CreateAssetMenu(menuName = "Tools/Configs/Inventory", fileName = "InventoryItem")]
    public class InventoryItemConfig : BaseScriptableConfig
    {
        public ItemType ItemType;

        public Vector2Int GridSize;
    }
}
