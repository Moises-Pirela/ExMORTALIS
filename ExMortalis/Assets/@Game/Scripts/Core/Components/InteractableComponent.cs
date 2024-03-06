using UnityEngine;
using UnityEngine.Events;

namespace Transendence.Core
{
    public class InteractableComponent : MonoBehaviour, IComponent
    {
        public UnityEvent OnInteract;
        public bool IsOpen = false;
        public bool IsUsed = false;
        public bool IsInspected = false;

        public ComponentType GetComponentType()
        {
            return ComponentType.Interactable;
        }
    }
}
