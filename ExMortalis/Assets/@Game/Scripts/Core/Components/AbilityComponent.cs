using System.Collections;
using System.Collections.Generic;
using Transendence.Core.Configs;
using UnityEngine;

namespace Transendence.Core
{
    public class AbilityComponent : MonoBehaviour, IComponent
    {
        public const int MAX_ABILITIES = 4;

        public AbilityConfig[] Abilities = new AbilityConfig[MAX_ABILITIES];

        [HideInInspector] public int AbilityCastedIndex = -1;

        public ComponentType GetComponentType()
        {
            return ComponentType.Ability;
        }

        public AbilityConfig GetAbilityCasted()
        {
            return Abilities[AbilityCastedIndex];
        }
    }
}
