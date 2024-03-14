using System.Collections.Generic;
using Transendence.Core.Postprocess;
using Transendence.Utilities;
using UnityEngine;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.PostProcess)]
    public class DamageSystem : BaseSystem
    {
        public const float DAMAGE_GIB_THRESHOLD = 50.0f;

        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            HealthComponent[] healthComponents = ((ComponentArray<HealthComponent>)componentArrays[(int)ComponentType.Health]).Components;
            GoreComponent[] goreComponents = ((ComponentArray<GoreComponent>)componentArrays[(int)ComponentType.Gore]).Components;
            DamageComponent[] damageComponents = ((ComponentArray<DamageComponent>)componentArrays[(int)ComponentType.Damage]).Components;

            for (int i = 0; i < postProcessEvents.Count; i++)
            {
                IPostProcessEvent postProcessEvent = postProcessEvents[i];
                if (postProcessEvent is DamagePostprocessEvent damageEvent)
                {
                    Entity entity = entities[damageEvent.TargetEntityId];

                    if (entity != null)
                    {
                        if (!entity.IsDead())
                        {
                            float healthPercentage = (damageEvent.Damage / healthComponents[entity.Id].CurrentHealth) * 100;

                            Debug.Log($"Health perc {healthPercentage}");

                            healthComponents[entity.Id].CurrentHealth -= damageEvent.Damage;

                            Debug.Log($"{entity.name} took {damageEvent.Damage} damage");

                            if (healthComponents[entity.Id].CurrentHealth <= 0)
                            {
                                if (goreComponents[entity.Id] != null && healthPercentage >= DAMAGE_GIB_THRESHOLD)
                                {
                                    goreComponents[entity.Id].GoreSimulator.ExecuteExplosion();
                                }
                                else
                                {
                                    goreComponents[entity.Id].GoreSimulator.ExecuteRagdoll(damageEvent.KnockbackForce);
                                }

                                KillEntityPostprocessEvent killEntityPostprocessEvent = new KillEntityPostprocessEvent
                                {
                                    EntityId = entity.Id,
                                    DestroyGO = true,
                                    DestroyTimer = 2
                                };

                                World.Instance.AddPostProcessEvent(killEntityPostprocessEvent);
                            }
                        }
                    }
                }
            }
        }
    }

}