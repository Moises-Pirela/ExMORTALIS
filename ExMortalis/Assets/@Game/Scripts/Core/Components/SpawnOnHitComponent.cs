using UnityEngine;

namespace NL.Core
{
    public class SpawnOnHitComponent : MonoBehaviour, IComponent
    {
        public GameObject SpawnEntity;

        public ComponentType GetComponentType() 
        {
            return ComponentType.SpawnOnHit;
        } 
    }
}
