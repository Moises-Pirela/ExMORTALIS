using NL.Utilities;
using Unity.Burst;
using UnityEngine;

namespace NL.Core.Systems
{
    [System(SystemAttributeType.Normal)]
    public class ThrowSystem : BaseSystem
    {
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

                ThrowableComponent throwableComponent = throwableComponents[throwerComponents[entity.Id].PickedUpEntityId];

                if (throwerComponents[entity.Id].ThrowDirection == UnityEngine.Vector3.zero)
                {
                    throwableComponent.transform.rotation = throwerComponents[entity.Id].transform.rotation;
                    throwableComponent.transform.position = throwerComponents[entity.Id].PickupTransform.position;
                    Physics.IgnoreCollision(throwerComponents[entity.Id].GetComponent<Collider>(), throwableComponent.GetComponent<Collider>(), true);
                    continue;
                }

                throwerComponents[entity.Id].PickedUpEntityId = -1;
                Physics.IgnoreCollision(throwerComponents[entity.Id].GetComponent<Collider>(), throwableComponent.GetComponent<Collider>(), false);

                throwableComponent.Rigidbody.useGravity = true;
                throwableComponent.transform.parent = null;

                throwableComponent.Rigidbody.AddForce(throwerComponents[entity.Id].ThrowDirection * throwerComponents[entity.Id].ThrowDistance, ForceMode.Impulse);

                throwerComponents[entity.Id].ThrowDirection = Vector3.zero;

            }
        }
    }

}