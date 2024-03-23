using NL.Core.Configs;
using NL.Utilities;
using Unity.Burst;
using UnityEngine;

namespace NL.Core.Systems
{
    [System(SystemAttributeType.Normal)]
    public class AbilitySystem : BaseSystem
    {
        [BurstCompile]
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays)
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

                    int abilityEntityId = World.Instance.EntityContainer.CreateEntity(abilityInstance); //TODO: Send to create entity system

                    if (abilityEntityId == -1)
                    {
                        abilityComponents[entity.Id].AbilityCastedIndex = -1;
                        continue;
                    }

                    Vector3 spawnPosition;
                    Vector3 forwardDirection;

                    if (World.Instance.EntityContainer.HasComponent<PlayerTagComponent>(entity.Id, ComponentType.PlayerTag))
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

                    if (World.Instance.EntityContainer.HasComponent<ThrowableComponent>(abilityEntityId, ComponentType.Throwable))
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
    }

}