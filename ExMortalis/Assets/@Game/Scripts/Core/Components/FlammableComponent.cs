using UnityEngine;

namespace Transendence.Core
{
    public class FlammableComponent : MonoBehaviour, IComponent
    {
        public bool IsOnFire;
        
        public ComponentType GetComponentType()
        {
            return ComponentType.Flammable;
        }
    }
}
