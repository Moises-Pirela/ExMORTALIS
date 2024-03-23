using UnityEngine;

namespace NL.Core
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
