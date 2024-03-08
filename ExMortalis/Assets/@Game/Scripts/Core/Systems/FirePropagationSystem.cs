using Transendence.Utilities;
using Unity.Burst;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.Normal)]
    public class FirePropagationSystem : BaseSystem
    {
        [BurstCompile]
        public override void Update(Entity[] entities, ComponentArray[] componentArrays)
        {
        }
    }

}