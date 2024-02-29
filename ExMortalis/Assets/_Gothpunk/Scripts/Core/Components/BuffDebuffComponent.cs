using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Transendence.Core
{
    public enum BuffDebuffType
    {
        Haste,
        Bleed,
        Poison,
        Damage,
        Health,
        Slow,
        Silence,
        MAX,
    }

    public enum BuffApplicationType
    {
        Base, //Note (Moises): Modifies base stat
        Flat, //Note (Moises) Add to the existing base stat
        Percentage //Note (Moises) Adds to the base stat by a percentage
    }

    public enum BuffDuplicationType
    {
        Ignore,
        Extend,
        Stack,
    }

    public struct BuffDebuff
    {
        public int BuffConfigId;
        public float ExpireTimeSec;
        public int Stacks;

        public float NextApplicationTime;

        public BuffDebuff(int type = -1, float expireTimeSec = -1, int stacks = -1)
        {
            BuffConfigId = type;
            ExpireTimeSec = expireTimeSec;
            Stacks = stacks;
            NextApplicationTime = 0;
        }

        public static BuffDebuff Empty { get { return new BuffDebuff() { BuffConfigId = -1 }; } }

        public static bool operator ==(BuffDebuff a, BuffDebuff b)
        {
            return a.BuffConfigId == b.BuffConfigId;
        }

        public static bool operator !=(BuffDebuff a, BuffDebuff b)
        {
            return a.BuffConfigId != b.BuffConfigId;
        }
    }

    public class BuffDebuffComponent : MonoBehaviour, IComponent
    {
        public BuffDebuff[] BuffDebuffs = new BuffDebuff[(int)BuffDebuffType.MAX];

        private void Awake()
        {
            for (int i = 0; i < BuffDebuffs.Length; i++) 
            {
                BuffDebuffs[i] = BuffDebuff.Empty;
            }
        }

        public ComponentType GetComponentType()
        {
            return ComponentType.BuffDebuff;
        }
    }
}
