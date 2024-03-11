using Transendence.Utilities;
using Unity.Burst;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.Fixed)]
    public class MovementSystem : BaseSystem
    {
        [BurstCompile]
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays)
        {

        }
    }

}