using UnityEngine;
using PampelGames.GoreSimulator;

namespace NL.Core
{
    public class GoreComponent : MonoBehaviour, IComponent
    {
        public GoreBone[] GoreObjects;
        public GoreSimulator GoreSimulator;
        public GameObject HitEffectPrefab;
        
        public ComponentType GetComponentType()
        {
            return ComponentType.Gore;
        }
    }

}
