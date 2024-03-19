using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Transendence.Core.Configs;
using UnityEngine;
using UnityEngine.Scripting;

namespace Transendence.Core
{
    public struct InventoryItem
    {
        public Vector2Int CurrentPosition;
        public int ConfigId;
        public int Stacks;
    }

    public class InventoryComponent : MonoBehaviour, IComponent
    {
        public Vector2Int InventorySize;
        public InventoryGrid InventoryGrid;

        public void Awake()
        {
            InventoryGrid = new InventoryGrid();

            InventoryGrid.InitializeGrid(InventorySize);
        }
        public ComponentType GetComponentType()
        {
            return ComponentType.Inventory;
        }
    }

    public class InventoryGrid
    {
        public Dictionary<int, List<Vector2Int>> ItemLocations = new Dictionary<int, List<Vector2Int>>();
        public InventoryItem[,] Inventory;
        public void InitializeGrid(Vector2Int gridSize)
        {
            Inventory = new InventoryItem[gridSize.x, gridSize.y];

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Inventory[x, y] = new InventoryItem() { ConfigId = -1 };
                }
            }
        }



        private bool TryAddToExistingStack(InventoryItem item)
        {
            if (ItemLocations.TryGetValue(item.ConfigId, out var positions))
            {
                foreach (var position in positions)
                {
                    if (Inventory[position.x, position.y].Stacks < World.Instance.WorldConfig.InventoryItemConfigs[item.ConfigId].MaxStacks)
                    {
                        Inventory[position.x, position.y].Stacks++;
                        Debug.Log($"Added to existing stack at {position.x}:{position.y}, new stack size: {Inventory[position.x, position.y].Stacks}");
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TryAdd(InventoryItem item)
        {
            bool addedToExistingStack = TryAddToExistingStack(item);
            if (addedToExistingStack)
            {
                return true;
            }

            bool addedAsNewStack = TryAddAsNewStack(item);
            if (addedAsNewStack)
            {
                return true;
            }

            Debug.LogError("Inventory is full or item cannot be added.");
            return false;
        }

        private bool TryAddAsNewStack(InventoryItem item)
        {
            Vector2Int size = World.Instance.WorldConfig.InventoryItemConfigs[item.ConfigId].GridSize;

            for (int x = 0; x < Inventory.GetLength(0) - size.x + 1; x++)
            {
                for (int y = 0; y < Inventory.GetLength(1) - size.y + 1; y++)
                {
                    bool isAvailable = true;
                    for (int i = 0; i < size.x; i++)
                    {
                        for (int j = 0; j < size.y; j++)
                        {
                            if (Inventory[x + i, y + j].ConfigId != -1)
                            {
                                isAvailable = false;
                                break;
                            }
                        }
                        if (!isAvailable) break;
                    }

                    if (isAvailable)
                    {
                        // Add the item to the inventory as a new stack
                        for (int i = 0; i < size.x; i++)
                        {
                            for (int j = 0; j < size.y; j++)
                            {
                                Inventory[x + i, y + j] = item;
                            }
                        }

                        item.Stacks = 1;

                        // Update the hash map with the new stack location
                        UpdateItemLocations(item, new Vector2Int(x, y));

                        Debug.Log($"Added new stack of {item.ConfigId} to {x}:{y}");
                        return true;
                    }
                }
            }

            return false;
        }


        public bool TryGetItemByIndex(int index, out InventoryItem item)
        {
            item = new InventoryItem() { ConfigId = -1 };
            int width = Inventory.GetLength(0);
            int height = Inventory.GetLength(1);

            int x = index % width;
            int y = index / width;

            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                item = Inventory[x, y];
                return true;
            }

            return false;
        }

        public bool TryGetGridPositionByIndex(int index, out Vector2Int position)
        {
            position = new Vector2Int();

            int width = Inventory.GetLength(0);
            int height = Inventory.GetLength(1);

            int x = index % width;
            int y = index / width;

            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                position = new Vector2Int(x, y);
                return true;
            }

            return false;
        }

        private void UpdateItemLocations(InventoryItem item, Vector2Int position)
        {
            item.CurrentPosition = position;

            if (!ItemLocations.ContainsKey(item.ConfigId))
            {
                ItemLocations[item.ConfigId] = new List<Vector2Int>();
            }

            ItemLocations[item.ConfigId].Add(position);
        }

        public void MoveItemToIndex(Vector2Int oldIndex, Vector2Int newIndex)
        {
            InventoryItem item = Inventory[oldIndex.x, oldIndex.y];

            Inventory[oldIndex.x, oldIndex.y] = new InventoryItem() { ConfigId = -1 };

            Inventory[newIndex.x, newIndex.y] = item;

            UpdateItemLocations(item, newIndex);
            RemoveItemLocation(item, oldIndex);

            Debug.Log($"Moved item {item.ConfigId} to index {newIndex}");

        }

        private void RemoveItemLocation(InventoryItem item, Vector2Int position)
        {
            if (ItemLocations.ContainsKey(item.ConfigId))
            {
                if (ItemLocations[item.ConfigId].Contains(position))
                {
                    ItemLocations[item.ConfigId].Remove(position);
                }
                else
                {
                    Debug.LogWarning($"Position {position} not found for item {item.ConfigId} in the item locations dictionary.");
                }
            }
            else
            {
                Debug.LogWarning($"Item {item.ConfigId} not found in the item locations dictionary.");
            }
        }

    }
}
