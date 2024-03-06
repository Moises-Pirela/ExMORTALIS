using System;
using System.Collections.Generic;
using System.Security;
using Transendence.Core.Configs;
using Transendence.Core.Postprocess;
using Transendence.Utilities;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UIElements;

namespace Transendence.Core
{
    public class World : MonoBehaviour
    {
        public static World Instance;
        public const int MAX_ENTITIES = 200;

        public EntityContainer EntityContainer;
        public WorldConfig WorldConfig;

        public GameObject PlayerPrefab;

        private List<Action<Entity[], ComponentArray[]>> Systems = new List<Action<Entity[], ComponentArray[]>>();
        private List<Action<Entity[], ComponentArray[], List<IPostProcessEvent>>> PostProcessSystems = new List<Action<Entity[], ComponentArray[], List<IPostProcessEvent>>>();
        private List<IPostProcessEvent> PostProcesses = new List<IPostProcessEvent>();

        public void Awake()
        {
            Instance = this;

            EntityContainer = new EntityContainer();

            Systems.Add(AbilitySystem);
            Systems.Add(CollisionSystem);
            Systems.Add(BuffSystem);
            PostProcessSystems.Add(DamageSystem);
            PostProcessSystems.Add(CreateEntitySystem);
            PostProcessSystems.Add(InteractionSystem);
        }

        public void Start()
        {
            // var playerEx = GameObject.Instantiate(PlayerPrefab) as GameObject;

            // EntityContainer.CreateEntity(playerEx);
        }

        public void AddPostProcessEvent(IPostProcessEvent processEvent)
        {
            PostProcesses.Add(processEvent);
        }

        private void Update()
        {
            foreach (var system in Systems)
            {
                system.Invoke(EntityContainer.Entities, EntityContainer.Components);
            }

            foreach (var system in PostProcessSystems)
            {
                system.Invoke(EntityContainer.Entities, EntityContainer.Components, PostProcesses);
            }

            PostProcesses.Clear();
        }

        #region Systems

        [BurstCompile]
        [System(SystemAttributeType.Normal)]
        public void AbilitySystem(Entity[] entities, ComponentArray[] componentArrays)
        {
            AbilityComponent[] abilityComponents = ((ComponentArray<AbilityComponent>)componentArrays[(int)ComponentType.Ability]).Components;
            ChildComponent[] childComponents = ((ComponentArray<ChildComponent>)componentArrays[(int)ComponentType.Child]).Components;
            ThrowableComponent[] throwableComponents = ((ComponentArray<ThrowableComponent>)componentArrays[(int)ComponentType.Throwable]).Components;
            PlayerTagComponent playerTagComponent = ((ComponentArray<PlayerTagComponent>)componentArrays[(int)ComponentType.PlayerTag]).Components[0];

            for (int i = 0; i < abilityComponents.Length; i++)
            {
                Entity entity = entities[i];

                if (entity == null || abilityComponents[i] == null) continue;

                if (!entity.IsActive()) continue;

                if (abilityComponents[i].AbilityCastedIndex != -1)
                {
                    AbilityConfig abilityConfig = abilityComponents[i].GetAbilityCasted();

                    var abilityInstance = GameObject.Instantiate(abilityConfig.AbilityPrefab) as GameObject;

                    int abilityEntityId = EntityContainer.CreateEntity(abilityInstance);

                    if (abilityEntityId == -1)
                    {
                        abilityComponents[entity.Id].AbilityCastedIndex = -1;
                        continue;
                    }

                    Vector3 spawnPosition;
                    Vector3 forwardDirection;

                    if (EntityContainer.HasComponent<PlayerTagComponent>(entity.Id, ComponentType.PlayerTag))
                    {
                        spawnPosition = playerTagComponent.GetForward() + playerTagComponent.transform.right + childComponents[abilityEntityId].SpawnOffset;
                        forwardDirection = playerTagComponent.PlayerCamera.transform.forward;
                    }
                    else
                    {
                        forwardDirection = entity.transform.forward;
                        spawnPosition = entity.transform.position + entity.transform.forward + childComponents[abilityEntityId].SpawnOffset;
                    }

                    abilityInstance.transform.position = spawnPosition;

                    if (EntityContainer.HasComponent<ThrowableComponent>(abilityEntityId, ComponentType.Throwable))
                    {
                        abilityInstance.transform.forward = forwardDirection;
                        throwableComponents[abilityEntityId].ThrowDirection = forwardDirection;
                        throwableComponents[abilityEntityId].SetThrow();
                    }

                    childComponents[abilityEntityId].ParentEntityId = entity.Id;

                    Debug.Log($"Entity {i} casted ability {abilityConfig.name}");
                    abilityComponents[i].AbilityCastedIndex = -1;
                }
            }
        }

        [System(SystemAttributeType.Normal)]
        public void PlayerSpawnSystem()
        {

        }

        [BurstCompile]
        [System(SystemAttributeType.Normal)]
        public void MovementSystem(Entity[] entities, ComponentArray[] componentArrays)
        {

        }

        [BurstCompile]
        [System(SystemAttributeType.Normal)]
        public void BuffSystem(Entity[] entities, ComponentArray[] componentArrays)
        {
            BuffDebuffComponent[] buffDebuffComponents = ((ComponentArray<BuffDebuffComponent>)componentArrays[(int)ComponentType.BuffDebuff]).Components;
            DamageComponent[] damageComponents = ((ComponentArray<DamageComponent>)componentArrays[(int)ComponentType.Damage]).Components;
            HealthComponent[] healthComponents = ((ComponentArray<HealthComponent>)componentArrays[(int)ComponentType.Health]).Components;

            for (int i = 0; i < buffDebuffComponents.Length; i++)
            {
                Entity entity = entities[i];

                if (entity == null || buffDebuffComponents[i] == null) continue;

                for (int j = 0; j < buffDebuffComponents[i].BuffDebuffs.Length; j++)
                {
                    BuffDebuff buffDebuff = buffDebuffComponents[i].BuffDebuffs[j];

                    if (buffDebuff == BuffDebuff.Empty) continue;

                    BuffDebuffConfig buffDebuffConfig = World.Instance.WorldConfig.BuffDebuffConfigs[buffDebuff.BuffConfigId];

                    if (buffDebuff.ExpireTimeSec < Time.time)
                    {
                        buffDebuffComponents[i].BuffDebuffs[j] = BuffDebuff.Empty;
                        Debug.Log($"Removed {World.Instance.WorldConfig.BuffDebuffConfigs[buffDebuff.BuffConfigId]}");
                        continue;
                    }

                    float baseBonus = buffDebuffConfig.BaseMagnitude * buffDebuff.Stacks;
                    float flatBonus = buffDebuffConfig.FlatMagnitude * buffDebuff.Stacks;
                    float percBonus = buffDebuffConfig.PercentageMagnitude * buffDebuff.Stacks;

                    switch (buffDebuffConfig.Type)
                    {
                        case BuffDebuffType.Haste:
                            break;
                        case BuffDebuffType.Slow:
                            break;
                        case BuffDebuffType.Bleed:
                            break;
                        case BuffDebuffType.Poison:
                            Debug.Log($"Health : {healthComponents[entity.Id].CurrentHealth}");
                            var buffedHealth = new BuffedValue<float>(healthComponents[entity.Id].CurrentHealth);
                            buffedHealth.FlatBonus = flatBonus * Time.deltaTime;
                            float healthLoss = Mathf.Clamp(buffedHealth.CalculateValue(), 0, healthComponents[entity.Id].MaxHealth.CalculateValue());
                            healthComponents[entity.Id].CurrentHealth = healthLoss;
                            Debug.Log($"Health after poison : {healthComponents[entity.Id].CurrentHealth}");
                            //TODO: Delegate to damage system
                            break;
                        case BuffDebuffType.Damage:
                            break;
                        case BuffDebuffType.Health:
                            break;
                        case BuffDebuffType.Silence:
                            break;
                    }
                }
            }
        }

        [BurstCompile]
        [System(SystemAttributeType.Normal)]
        public void FirePropagationSystem(Entity[] entities, ComponentArray[] componentArrays)
        {
            FlammableComponent[] flammableComponents = ((ComponentArray<FlammableComponent>)componentArrays[(int)ComponentType.Flammable]).Components;

            for (int i = 0; i < flammableComponents.Length; i++)
            {
                Entity entity = entities[i];

                FlammableComponent flammableComponent = flammableComponents[i];

                if (entity == null || flammableComponent == null) continue;
            }
        }

        [BurstCompile]
        [System(SystemAttributeType.Normal)]
        public void CollisionSystem(Entity[] entities, ComponentArray[] componentArrays)
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

                    if (EntityContainer.GetComponent(entity.Id, ComponentType.DamageOnHit, out DamageOnHitComponent damageOnHitComponent))
                    {
                        if (EntityContainer.HasComponent<HealthComponent>(collidedEntity.Id, ComponentType.Health))
                        {
                            DamagePostprocessEvent damagePostprocessEvent = new DamagePostprocessEvent();

                            damagePostprocessEvent.TargetEntityId = collidedEntity.Id;
                            damagePostprocessEvent.DamageDealerEntityId = entity.Id;

                            PostProcesses.Add(damagePostprocessEvent);
                        }
                    }

                    if (EntityContainer.GetComponent(entity.Id, ComponentType.SpawnOnHit, out SpawnOnHitComponent spawnOnHitComponent))
                    {
                        var spawnedInstance = GameObject.Instantiate(spawnOnHitComponent.SpawnEntity) as GameObject;

                        int spawnedEntityId = EntityContainer.CreateEntity(spawnedInstance);

                        entities[spawnedEntityId].transform.position = collision.Value.ContactPoint;
                    }

                    if (EntityContainer.GetComponent(entity.Id, ComponentType.BuffOnHit, out BuffOnHitComponent buffOnHit))
                    {
                        BuffDebuffConfig buffConfig = buffOnHitComponents[entity.Id].BuffDebuffConfig;

                        if (!EntityContainer.GetComponent(collidedEntity.Id, ComponentType.BuffDebuff, out BuffDebuffComponent buff)) return;

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

                    if (EntityContainer.GetComponent(entity.Id, ComponentType.DestroyOnHit, out DestroyOnHitComponent destroyOnHitComponent))
                    {
                        EntityContainer.RemoveEntity(entity.Id);
                    }
                }
            }
        }

        #endregion

        #region Postprocess event systems

        [System(SystemAttributeType.PostProcess)]
        public void DamageSystem(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            HealthComponent[] healthComponents = ((ComponentArray<HealthComponent>)componentArrays[(int)ComponentType.Health]).Components;
            DamageComponent[] damageComponents = ((ComponentArray<DamageComponent>)componentArrays[(int)ComponentType.Damage]).Components;

            foreach (var postProcessEvent in postProcessEvents)
            {
                if (postProcessEvent is DamagePostprocessEvent damageEvent)
                {
                    Entity entity = entities[damageEvent.TargetEntityId];

                    if (entity != null)
                    {
                        if (!entity.IsDead())
                        {
                            if (damageComponents[damageEvent.DamageDealerEntityId].NextDamageTime > Time.time) continue;

                            healthComponents[entity.Id].CurrentHealth -= damageComponents[damageEvent.DamageDealerEntityId].BaseDamage;
                            damageComponents[damageEvent.DamageDealerEntityId].NextDamageTime = Time.time + damageComponents[damageEvent.DamageDealerEntityId].DamageTime;

                            Debug.Log($"{entity.name} took {damageComponents[damageEvent.DamageDealerEntityId].BaseDamage} damage");
                        }
                    }
                }
            }
        }

        [System(SystemAttributeType.PostProcess)]
        public void InteractionSystem(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
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

        [System(SystemAttributeType.PostProcess)]
        public void CreateEntitySystem(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {

        }

        [System(SystemAttributeType.PostProcess)]
        public void KillEntitySystem(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
        }

        #endregion
    }
}