using UnityEngine;

namespace NL.Core
{
    public class ChildComponent : MonoBehaviour, IComponent
    {
        public int ParentEntityId;
        public Vector3 SpawnOffset;
        public Vector3 SpawnRotation;

        public ComponentType GetComponentType()
        {
            return ComponentType.Child;
        }
    }
}
