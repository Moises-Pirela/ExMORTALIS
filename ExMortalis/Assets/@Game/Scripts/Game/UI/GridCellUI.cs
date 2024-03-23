using TMPro;
using Transendence.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Transendence.Game.UI
{
    public class GridCellUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
    {
        public Vector2Int GridPosition;
        private RectTransform rectTransform;
        private Canvas canvas;
        private CanvasGroup canvasGroup;
        private Vector3 originalPosition;
        private GameObject DraggedItemObject;
        private TextMeshProUGUI Text;

        public void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            originalPosition = rectTransform.position;
            Text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetItem(Sprite image, string text)
        {
            GetComponent<Image>().sprite = image;
            Text.text = text;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                StartDragging();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (DraggedItemObject != null)
            {
                rectTransform.position = UnityEngine.Input.mousePosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (DraggedItemObject != null)
            {
                StopDragging();
            }
        }

        private void StartDragging()
        {
            if (DraggedItemObject == null)
            {
                DraggedItemObject = gameObject;
                canvasGroup.alpha = 0.6f;
                canvasGroup.blocksRaycasts = false;
            }
        }

        private void StopDragging()
        {
            if (DraggedItemObject != null)
            {
                Vector2Int dropIndex = CalculateDropIndex();

                if (dropIndex.x == -1)
                {
                    Reset();
                    return;
                }

                World.Instance.EntityContainer.GetComponent(World.PLAYER_ENTITY_ID, ComponentType.Inventory, out InventoryComponent playerInventory);

                playerInventory.Inventory.MoveItemToIndex(GridPosition, dropIndex);

                DraggedItemObject = null;

                GameManager.Instance.UIManager.SendUpdateCommand(UICommand.TabMenuUpdate, new InventoryUIData());

                Reset();
            }
        }

        private void Reset()
        {
            rectTransform.position = originalPosition;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        private Vector2Int CalculateDropIndex()
        {
            World.Instance.EntityContainer.GetComponent(World.PLAYER_ENTITY_ID, ComponentType.Inventory, out InventoryComponent playerInventory);

            if (CheckCollisionAndGetPosition(out Vector2Int collision))
            {
                return collision;
            }

            return new Vector2Int(-1,-1);
        }

        private bool CheckCollisionAndGetPosition(out Vector2Int collision)
        {
            GridCellUI[] allGridCells = transform.parent.GetComponentsInChildren<GridCellUI>();
            collision = new Vector2Int();

            foreach (GridCellUI otherGridCell in allGridCells)
            {
                if (otherGridCell == this)
                    continue;

                if (RectTransformUtility.RectangleContainsScreenPoint(otherGridCell.rectTransform, rectTransform.position))
                {
                    collision = otherGridCell.GridPosition;
                    return true;
                }
            }


            return false;
        }

    }
}
