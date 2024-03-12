using JetBrains.Annotations;
using Transendence.Core.Configs;
using UnityEngine;
using UnityEngine.Scripting;

namespace Transendence.Core
{
    public class InventoryComponent : MonoBehaviour, IComponent
    {
        public AmmoCount[] AmmoCounts = new AmmoCount[(int)AmmoType.Max];
        public Vector2Int InventorySize;
        public InventoryGrid InventoryGrid;

        public void Awake()
        {
            for (int i = 0; i < AmmoCounts.Length; i++)
            {
                AmmoCounts[i] = new AmmoCount { CurrentCount = 0, MaxCount = 0, AmmoType = (AmmoType)i };
            }

            InventoryGrid.InitializeGrid(InventorySize);
        }
        public ComponentType GetComponentType()
        {
            return ComponentType.Inventory;
        }
    }

    public struct InventoryGrid
    {
        public InventoryItem[,] Inventory;
        public void InitializeGrid(Vector2Int gridSize)
        {
            Inventory = new InventoryItem[gridSize.x, gridSize.y];

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Inventory[x, y] = new InventoryItem();
                }
            }
        }
    }

    public struct InventoryItem
    {
        int ConfigId;
    }
}
