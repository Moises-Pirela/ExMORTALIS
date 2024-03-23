using NL.Utilities;
using Unity.Burst;

namespace NL.Core.Systems
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