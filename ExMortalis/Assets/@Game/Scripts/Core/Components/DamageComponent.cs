using UnityEngine;

namespace NL.Core
{
    public enum DamageType
    {
        KINETIC, NEUROBIOTIC, ENERGY
    }

    public enum DamageMethod
    {
        AOE,
        PROJECTILE,
        PIERCE
    }

    public enum DamageApplication
    {
        DIRECT,
        OVER_TIME,
    }

    public class DamageComponent : MonoBehaviour, IComponent
    {
        public DamageMethod DamageMethod;
        public DamageType DamageType;
        public DamageApplication DamageApplication;
        public float DamageTime;
        public float NextDamageTime;
        public float BaseDamage;

        private void Awake()
        {
            NextDamageTime = Time.time;
        }

        public ComponentType GetComponentType()
        {
            return ComponentType.Damage;
        }
    }
}
