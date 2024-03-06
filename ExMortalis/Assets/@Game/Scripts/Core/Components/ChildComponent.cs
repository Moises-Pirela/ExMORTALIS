using UnityEngine;

namespace Transendence.Core
{
    public class ChildComponent : MonoBehaviour, IComponent
    {
        public int ParentEntityId;
        public Vector3 SpawnOffset;

        public ComponentType GetComponentType()
        {
            return ComponentType.Child;
        }
    }
}
