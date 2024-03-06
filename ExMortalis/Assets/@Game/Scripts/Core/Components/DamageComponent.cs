using UnityEngine;

namespace Transendence.Core
{
    public enum DamageType
    {
        Physical,
        Chemical,
    }

    public enum DamageMethod
    {
        AOE,
        Projectile,
        Pierce
    }

    public enum DamageApplication
    {
        Direct,
        Over_Time,
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
