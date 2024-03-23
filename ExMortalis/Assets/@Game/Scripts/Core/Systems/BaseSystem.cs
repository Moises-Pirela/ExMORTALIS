using System.Collections.Generic;
using NL.Core.Postprocess;

namespace NL.Core.Systems
{
    public abstract class BaseSystem
    {
        public virtual void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays)
        { }
        public virtual void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        { }
    }

}