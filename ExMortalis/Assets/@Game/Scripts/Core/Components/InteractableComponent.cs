using UnityEngine;
using UnityEngine.Events;

namespace NL.Core
{
    public class InteractableComponent : MonoBehaviour, IComponent
    {
        public UnityEvent OnInteract;
        public bool IsOpen = false;
        public bool IsUsed = false;
        public bool IsInspected = false;
        public bool IsPickedUp = false;

        public ComponentType GetComponentType()
        {
            return ComponentType.Interactable;
        }
    }
}
