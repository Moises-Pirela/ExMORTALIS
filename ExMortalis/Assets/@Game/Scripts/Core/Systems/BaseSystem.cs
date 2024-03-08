using System.Collections.Generic;
using Transendence.Core.Postprocess;

namespace Transendence.Core.Systems
{
    public abstract class BaseSystem
    {
        public virtual void Update(Entity[] entities, ComponentArray[] componentArrays)
        { }
        public virtual void Update(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        { }
    }

}