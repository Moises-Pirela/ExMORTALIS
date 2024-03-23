using System.Collections.Generic;
using NL.Core.Postprocess;
using NL.Utilities;

namespace NL.Core.Systems
{
    [System(SystemAttributeType.PostProcess)]
    public class GoreSystem : BaseSystem
    {
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            HealthComponent[] healthComponents = ((ComponentArray<HealthComponent>)componentArrays[(int)ComponentType.Health]).Components;
            GoreComponent[] goreComponents = ((ComponentArray<GoreComponent>)componentArrays[(int)ComponentType.Gore]).Components;
            
        }
    }

}