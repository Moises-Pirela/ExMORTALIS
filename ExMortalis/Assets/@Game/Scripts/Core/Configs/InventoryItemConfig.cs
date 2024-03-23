using UnityEngine;
using Unity.Mathematics;

namespace NL.Core.Configs
{
    public enum ItemType
    {
        Junk,
        Equippable,
        Ammo,
    }

    [CreateAssetMenu(menuName = "Tools/Configs/Inventory", fileName = "InventoryItem")]
    public class InventoryItemConfig : BaseScriptableConfig
    {
        [Header("Inventory")]
        public Sprite ItemSprite;
        public ItemType ItemType;
        public Vector2Int GridSize;
        public int MaxStacks;
    }
}
