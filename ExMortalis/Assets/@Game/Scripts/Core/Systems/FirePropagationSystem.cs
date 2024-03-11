using Transendence.Utilities;
using Unity.Burst;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.Normal)]
    public class FirePropagationSystem : BaseSystem
    {
        [BurstCompile]
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays)
        {
        }
    }

}