using Transendence.Core.Configs;
using UnityEngine;

namespace Transendence.Core
{
    public class PickUpOnInteractComponent : MonoBehaviour, IComponent
    {
        public InventoryItemConfig InventoryItemConfig;

        public ComponentType GetComponentType()
        {
            return ComponentType.PickUpOnInteract;
        }
    }
}
