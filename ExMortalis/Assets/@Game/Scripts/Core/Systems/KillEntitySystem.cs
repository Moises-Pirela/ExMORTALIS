using System.Collections.Generic;
using NL.Core.Postprocess;
using NL.Utilities;
using Unity.Mathematics;

namespace NL.Core.Systems
{
    [System(SystemAttributeType.PostProcess, priority: int.MaxValue)]
    public class KillEntitySystem : BaseSystem
    {
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            for (int i1 = 0; i1 < postProcessEvents.Count; i1++)
            {
                IPostProcessEvent postProcessEvent = postProcessEvents[i1];

                if (postProcessEvent is KillEntityPostprocessEvent killEntity)
                {
                    World.Instance.EntityContainer.RemoveEntity(killEntity.EntityId, killEntity.DestroyGO, killEntity.DestroyTimer);
                }
            }
        }
    }

}