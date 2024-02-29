using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Transendence.Core
{
    public class HealthComponent : MonoBehaviour, IComponent
    {
        [SerializeField] public BuffedValue<float> MaxHealth;
        [HideInInspector] public float CurrentHealth;

        private void Awake()
        {
            CurrentHealth = MaxHealth.CalculateValue();
        }

        public ComponentType GetComponentType()
        {
            return ComponentType.Health;
        }
    }
}


