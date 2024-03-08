using UnityEngine;

namespace Transendence.Core
{
    public class InventoryComponent : MonoBehaviour, IComponent
    {
        public int InventorySize;
        public ComponentType GetComponentType()
        {
            return ComponentType.Inventory;
        }
    }
}
