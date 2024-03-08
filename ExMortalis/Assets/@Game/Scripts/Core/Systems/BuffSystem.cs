using Transendence.Core.Configs;
using Transendence.Utilities;
using Unity.Burst;
using UnityEngine;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.Normal)]
    public class BuffSystem : BaseSystem
    {
        [BurstCompile]
        public override void Update(Entity[] entities, ComponentArray[] componentArrays)
        {
            BuffDebuffComponent[] buffDebuffComponents = ((ComponentArray<BuffDebuffComponent>)componentArrays[(int)ComponentType.BuffDebuff]).Components;
            DamageComponent[] damageComponents = ((ComponentArray<DamageComponent>)componentArrays[(int)ComponentType.Damage]).Components;
            HealthComponent[] healthComponents = ((ComponentArray<HealthComponent>)componentArrays[(int)ComponentType.Health]).Components;

            for (int i = 0; i < buffDebuffComponents.Length; i++)
            {
                Entity entity = entities[i];

                if (entity == null || buffDebuffComponents[i] == null) continue;

                for (int j = 0; j < buffDebuffComponents[i].BuffDebuffs.Length; j++)
                {
                    BuffDebuff buffDebuff = buffDebuffComponents[i].BuffDebuffs[j];

                    if (buffDebuff == BuffDebuff.Empty) continue;

                    BuffDebuffConfig buffDebuffConfig = World.Instance.WorldConfig.BuffDebuffConfigs[buffDebuff.BuffConfigId];

                    if (buffDebuff.ExpireTimeSec < Time.time)
                    {
                        buffDebuffComponents[i].BuffDebuffs[j] = BuffDebuff.Empty;
                        Debug.Log($"Removed {World.Instance.WorldConfig.BuffDebuffConfigs[buffDebuff.BuffConfigId]}");
                        continue;
                    }

                    float baseBonus = buffDebuffConfig.BaseMagnitude * buffDebuff.Stacks;
                    float flatBonus = buffDebuffConfig.FlatMagnitude * buffDebuff.Stacks;
                    float percBonus = buffDebuffConfig.PercentageMagnitude * buffDebuff.Stacks;

                    switch (buffDebuffConfig.Type)
                    {
                        case BuffDebuffType.Haste:
                            break;
                        case BuffDebuffType.Slow:
                            break;
                        case BuffDebuffType.Bleed:
                            break;
                        case BuffDebuffType.Poison:
                            Debug.Log($"Health : {healthComponents[entity.Id].CurrentHealth}");
                            var buffedHealth = new BuffedValue<float>(healthComponents[entity.Id].CurrentHealth);
                            buffedHealth.FlatBonus = flatBonus * Time.deltaTime;
                            float healthLoss = Mathf.Clamp(buffedHealth.CalculateValue(), 0, healthComponents[entity.Id].MaxHealth.CalculateValue());
                            healthComponents[entity.Id].CurrentHealth = healthLoss;
                            Debug.Log($"Health after poison : {healthComponents[entity.Id].CurrentHealth}");
                            //TODO: Delegate to damage system
                            break;
                        case BuffDebuffType.Damage:
                            break;
                        case BuffDebuffType.Health:
                            break;
                        case BuffDebuffType.Silence:
                            break;
                    }
                }
            }
        }
    }

}