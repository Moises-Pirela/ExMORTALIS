using Transendence.Core;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Transendence.Game.UI
{
    public struct InventoryUIData : IUIData
    {

    }

    public class UITabMenu : BaseUI, IUIWithData
    {
        public static string UI_PATH = "UI/UI_TabMenu";

        public GridCellUI[] GridSlots;

        public override void Initialize(bool show)
        {
            base.Initialize(show);


        }

        public override void Show()
        {
            base.Show();

            UpdateUI(new InventoryUIData());

            World.Instance.EntityContainer.GetComponent(World.PLAYER_ENTITY_ID, ComponentType.Inventory, out InventoryComponent playerInventory);

            for (int i = 0; i < GridSlots.Length; i++)
            {
                if (playerInventory.InventoryGrid.TryGetGridPositionByIndex(i, out Vector2Int pos))
                {
                    GridSlots[i].GridPosition = pos;
                }
            }
        }

        public void UpdateUI(IUIData uIData)
        {
            World.Instance.EntityContainer.GetComponent(World.PLAYER_ENTITY_ID, ComponentType.Inventory, out InventoryComponent playerInventory);

            for (int i = 0; i < GridSlots.Length; i++)
            {
                InventoryItem inventoryItem = playerInventory.InventoryGrid.Inventory[GridSlots[i].GridPosition.x, GridSlots[i].GridPosition.y];

                Sprite itemImage = null;
                string text = "EMPTY";

                if (inventoryItem.ConfigId != -1)
                {
                    itemImage = World.Instance.WorldConfig.InventoryItemConfigs[inventoryItem.ConfigId].ItemSprite;
                    text = World.Instance.WorldConfig.InventoryItemConfigs[inventoryItem.ConfigId].Name;
                }

                GridSlots[i].SetItem(itemImage, text);
            }
        }
    }
}
