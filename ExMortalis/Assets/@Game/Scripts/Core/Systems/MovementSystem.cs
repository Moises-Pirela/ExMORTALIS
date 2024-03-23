using NL.Utilities;
using Unity.Burst;

namespace NL.Core.Systems
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