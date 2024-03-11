using System.Collections.Generic;
using Transendence.Core.Postprocess;
using Transendence.Utilities;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.PostProcess)]
    public class CreateEntitySystem : BaseSystem
    {
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {

        }
    }

}