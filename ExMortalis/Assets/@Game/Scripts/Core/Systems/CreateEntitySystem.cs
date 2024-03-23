using System.Collections.Generic;
using NL.Core.Postprocess;
using NL.Utilities;
using Unity.Burst;
using UnityEngine;

namespace NL.Core.Systems
{
    [System(SystemAttributeType.PostProcess, -1)]
    public class CreateEntitySystem : BaseSystem
    {
        [BurstCompile]
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            for (int i1 = 0; i1 < postProcessEvents.Count; i1++)
            {
                IPostProcessEvent postProcessEvent = postProcessEvents[i1];

                if (postProcessEvent is CreateEntityPostprocessEvent createEntity)
                {
                    int entityID = World.Instance.EntityContainer.CreateEntity(createEntity.EntityGO);

                    if (World.Instance.EntityContainer.HasComponent<PlayerTagComponent>(entityID, ComponentType.PlayerTag))
                    {
                        World.PLAYER_ENTITY_ID = entityID;
                    }
                }
            }
        }
    }

}