using Transendence.Core.Configs;
using Transendence.Core.Postprocess;
using Transendence.Utilities;
using Unity.Burst;
using UnityEngine;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.Fixed)]
    public class CollisionSystem : BaseSystem
    {
        [BurstCompile]
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays)
        {
            CollisionComponent[] collisionComponents = ((ComponentArray<CollisionComponent>)componentArrays[(int)ComponentType.Collision]).Components;
            BuffDebuffComponent[] buffDebuffComponents = ((ComponentArray<BuffDebuffComponent>)componentArrays[(int)ComponentType.BuffDebuff]).Components;
            BuffOnHitComponent[] buffOnHitComponents = ((ComponentArray<BuffOnHitComponent>)componentArrays[(int)ComponentType.BuffOnHit]).Components;

            for (int i = 0; i < collisionComponents.Length; i++)
            {
                Entity entity = entities[i];

                CollisionComponent collisionComponent = collisionComponents[i];

                if (entity == null || collisionComponent == null) continue;

                if (!entity.IsActive()) continue;

                foreach (var collision in collisionComponent.Collisions)
                {
                    Entity collidedEntity = entities[collision.Key];

                    if (collidedEntity.IsDead() || !collidedEntity.IsActive()) continue;

                    if (World.Instance.EntityContainer.GetComponent(entity.Id, ComponentType.DamageOnHit, out DamageOnHitComponent damageOnHitComponent))
                    {
                        if (World.Instance.EntityContainer.HasComponent<HealthComponent>(collidedEntity.Id, ComponentType.Health))
                        {
                            DamagePostprocessEvent damagePostprocessEvent = new DamagePostprocessEvent();

                            damagePostprocessEvent.TargetEntityId = collidedEntity.Id;
                            damagePostprocessEvent.DamageDealerEntityId = entity.Id;

                            World.Instance.AddPostProcessEvent(damagePostprocessEvent);
                        }
                    }

                    if (World.Instance.EntityContainer.GetComponent(entity.Id, ComponentType.SpawnOnHit, out SpawnOnHitComponent spawnOnHitComponent))
                    {
                        var spawnedInstance = GameObject.Instantiate(spawnOnHitComponent.SpawnEntity) as GameObject;

                        int spawnedEntityId = World.Instance.EntityContainer.CreateEntity(spawnedInstance);

                        entities[spawnedEntityId].transform.position = collision.Value.ContactPoint;
                    }

                    if (World.Instance.EntityContainer.GetComponent(entity.Id, ComponentType.BuffOnHit, out BuffOnHitComponent buffOnHit))
                    {
                        BuffDebuffConfig buffConfig = buffOnHitComponents[entity.Id].BuffDebuffConfig;

                        if (!World.Instance.EntityContainer.GetComponent(collidedEntity.Id, ComponentType.BuffDebuff, out BuffDebuffComponent buff)) return;

                        if (buffDebuffComponents[collidedEntity.Id].BuffDebuffs[(int)buffConfig.Type] != BuffDebuff.Empty)
                        {
                            if (buffDebuffComponents[collidedEntity.Id].BuffDebuffs[(int)buffConfig.Type].NextApplicationTime > Time.time) return;

                            switch (buffConfig.DuplicationType)
                            {
                                case BuffDuplicationType.Ignore:
                                    break;
                                case BuffDuplicationType.Extend:
                                    buffDebuffComponents[collidedEntity.Id].BuffDebuffs[(int)buffConfig.Type].ExpireTimeSec = Mathf.Clamp(buffDebuffComponents[collidedEntity.Id].BuffDebuffs[(int)buffConfig.Type].ExpireTimeSec + buffConfig.DurationSec, 0, Time.time + buffConfig.MaxDuration);
                                    Debug.Log($"{buffConfig.Type} has extended {buffDebuffComponents[collidedEntity.Id].BuffDebuffs[(int)buffConfig.Type].ExpireTimeSec}");
                                    break;
                                case BuffDuplicationType.Stack:
                                    buffDebuffComponents[collidedEntity.Id].BuffDebuffs[(int)buffConfig.Type].Stacks = Mathf.Clamp(buffDebuffComponents[collidedEntity.Id].BuffDebuffs[(int)buffConfig.Type].Stacks + buffConfig.StacksToApply, 0, buffConfig.MaxStacks);
                                    Debug.Log($"{buffConfig.Type} has stacked {buffDebuffComponents[collidedEntity.Id].BuffDebuffs[(int)buffConfig.Type].Stacks}");
                                    break;
                            }

                            buffDebuffComponents[collidedEntity.Id].BuffDebuffs[(int)buffConfig.Type].NextApplicationTime = Time.time + 1;
                        }
                        else
                        {
                            BuffDebuff buffDebuff = new BuffDebuff()
                            {
                                Stacks = buffConfig.StacksToApply,
                                ExpireTimeSec = buffConfig.DurationSec + Time.time,
                                BuffConfigId = (int)buffConfig.Id,
                                NextApplicationTime = Time.time + 1,
                            };

                            buffDebuffComponents[collidedEntity.Id].BuffDebuffs[(int)buffConfig.Type] = buffDebuff;

                            Debug.Log($"Applied {buffConfig.Type}");
                        }


                    }

                    if (World.Instance.EntityContainer.GetComponent(entity.Id, ComponentType.DestroyOnHit, out DestroyOnHitComponent destroyOnHitComponent))
                    {
                        World.Instance.EntityContainer.RemoveEntity(entity.Id);
                    }
                }
            }
        }
    }

}