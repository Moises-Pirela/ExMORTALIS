using System.Collections.Generic;
using Transendence.Core.Postprocess;
using Transendence.Utilities;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.PostProcess)]
    public class InteractionSystem : BaseSystem
    {
        public override void Update(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            InteractableComponent[] interactableComponents = ((ComponentArray<InteractableComponent>)componentArrays[(int)ComponentType.Interactable]).Components;
            foreach (var postProcessEvent in postProcessEvents)
            {
                if (postProcessEvent is InteractPostprocessEvent interactPostprocess)
                {
                    interactableComponents[interactPostprocess.TargetEntityId].OnInteract.Invoke();
                }
            }
        }
    }

}