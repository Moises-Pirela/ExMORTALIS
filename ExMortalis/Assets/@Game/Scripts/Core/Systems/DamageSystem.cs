using System.Collections.Generic;
using Transendence.Core.Postprocess;
using Transendence.Utilities;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.PostProcess)]
    public class DamageSystem : BaseSystem
    {
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
             HealthComponent[] healthComponents = ((ComponentArray<HealthComponent>)componentArrays[(int)ComponentType.Health]).Components;
            DamageComponent[] damageComponents = ((ComponentArray<DamageComponent>)componentArrays[(int)ComponentType.Damage]).Components;

            foreach (var postProcessEvent in postProcessEvents)
            {
                if (postProcessEvent is DamagePostprocessEvent damageEvent)
                {
                    Entity entity = entities[damageEvent.TargetEntityId];

                    if (entity != null)
                    {
                        if (!entity.IsDead())
                        {
                            healthComponents[entity.Id].CurrentHealth -= damageEvent.Damage;
                        }
                    }
                }
            }
        }
    }

}