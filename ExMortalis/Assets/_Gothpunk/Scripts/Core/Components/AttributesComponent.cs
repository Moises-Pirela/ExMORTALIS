using UnityEngine;

namespace Transendence.Core
{
    public class AttributesComponent : MonoBehaviour, IComponent
    {
        public BuffedValue<float> Strength;
        public BuffedValue<float> Agility;
        public BuffedValue<float> Intellect;
        public BuffedValue<float> Charisma;

        public ComponentType GetComponentType()
        {
            return ComponentType.Attributes;
        }
    }
}
