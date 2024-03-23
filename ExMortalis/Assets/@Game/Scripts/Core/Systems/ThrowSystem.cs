using NL.Utilities;
using Unity.Burst;
using UnityEngine;

namespace NL.Core.Systems
{
    [System(SystemAttributeType.Normal)]
    public class ThrowSystem : BaseSystem
    {
        //TODO: Check for the entities that throw things not the ones that get thrown
        [BurstCompile]
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays)
        {
            ThrowerComponent[] throwerComponents = ((ComponentArray<ThrowerComponent>)componentArrays[(int)ComponentType.Thrower]).Components;
            ThrowableComponent[] throwableComponents = ((ComponentArray<ThrowableComponent>)componentArrays[(int)ComponentType.Throwable]).Components;

            for (int i = 0; i < throwerComponents.Length; i++)
            {
                Entity entity = entities[i];

                if (entity == null || throwerComponents[i] == null || !entity.IsActive() || entity.IsDead()) continue;

                if (throwerComponents[entity.Id].PickedUpEntityId == -1) continue;

                if (throwerComponents[entity.Id].ThrowDirection == UnityEngine.Vector3.zero) continue;

                ThrowableComponent throwableComponent = throwableComponents[throwerComponents[entity.Id].PickedUpEntityId];

                throwableComponent.Rigidbody.isKinematic = true;
                throwableComponent.Throw(throwerComponents[entity.Id].ThrowDistance, throwerComponents[entity.Id].ThrowDirection);

                throwerComponents[entity.Id].PickedUpEntityId = -1;
                throwerComponents[entity.Id].ThrowDirection = Vector3.zero;
                throwerComponents[entity.Id].ThrowDistance = 0;
            }
        }
    }

}