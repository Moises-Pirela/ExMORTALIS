using NL.Utilities;
using System.Linq;
using Unity.Burst;
using UnityEngine;

namespace NL.Core.Systems
{
    [System(SystemAttributeType.Normal)]
    public class KickSystem : BaseSystem
    {
        [BurstCompile]
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays)
        {
            KickerComponent[] kickerComponents = ((ComponentArray<KickerComponent>)componentArrays[(int)ComponentType.Kicker]).Components;
            KickableComponent[] kickableComponents = ((ComponentArray<KickableComponent>)componentArrays[(int)ComponentType.Kickable]).Components;

            for (int i = 0; i < kickerComponents.Length; i++)
            {
                if (kickerComponents[i] != null)
                {
                    Entity entity = entities[i];

                    if (entity == null || kickerComponents[i] == null || !entity.IsActive() || entity.IsDead()) continue;

                    if (kickerComponents[entity.Id].HasKicked)
                    {
                        if (kickerComponents[entity.Id].NextKickTime < Time.time)
                        {
                            Ray ray = new Ray(kickerComponents[entity.Id].KickTransform.position, kickerComponents[entity.Id].KickTransform.forward);

                            RaycastHit raycastHit;
                            Physics.Raycast(ray, out raycastHit, kickerComponents[entity.Id].KickRange, kickerComponents[entity.Id].KickMask, QueryTriggerInteraction.Ignore);
                            //Physics.SphereCast(ray.origin, kickerComponents[entity.Id].KickRadius, ray.direction, out raycastHit, kickerComponents[entity.Id].KickRange, kickerComponents[entity.Id].KickMask, QueryTriggerInteraction.Ignore);

                            if (raycastHit.collider != null)
                            {
                                Entity kickedEntity = raycastHit.collider.GetComponent<Entity>();

                                if (kickedEntity != null)
                                {
                                    if (World.Instance.EntityContainer.HasComponent<KickableComponent>(kickedEntity.Id, ComponentType.Kickable))
                                    {
                                        kickableComponents[kickedEntity.Id].Rigidbody.AddForce(kickerComponents[entity.Id].KickTransform.forward * kickerComponents[entity.Id].KickForce, ForceMode.Impulse);
                                        kickableComponents[kickedEntity.Id].PlayHitSound();
                                        Debug.Log($"Kicked {kickedEntity}");
                                    }
                                }
                                else
                                {
                                    Debug.Log($"Kicked nothing");
                                }
                            }

                            kickerComponents[entity.Id].NextKickTime = Time.time + kickerComponents[entity.Id].KickCooldownTimeSecs;
                        }
                        else
                        {
                            Debug.Log($"Kick on cooldown");
                        }
                            kickerComponents[entity.Id].HasKicked = false;
                    }
                }
            }
        }
    }

}