using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

namespace Transendence.Core
{
    public class EntityContainer
    {
        public Entity[] Entities;
        public ComponentArray[] Components;

        private int AvailableEntityId;
        private int RecycledEntityId = -1;

        public EntityContainer()
        {
            Entities = new Entity[World.MAX_ENTITIES];
            Components = new ComponentArray[(int)ComponentType.Max];

            for (int i = 0; i < Components.Length; i++)
            {
                Components[i] = GetComponentArray((ComponentType)i);
            }
        }

        public bool HasComponent<T>(int entityId, ComponentType componentType) where T : IComponent
        {
            if (entityId == -1) return false;

            T[] array = ((ComponentArray<T>)Components[(int)componentType]).Components;

            return array[entityId] != null;
        }

        public bool GetComponent<T>(int entityId, ComponentType componentType, out T component) where T : IComponent
        {
            component = default(T);

            if (!HasComponent<T>(entityId, componentType)) return false;

            T[] array = ((ComponentArray<T>)Components[(int)componentType]).Components;

            component = array[entityId];

            return true;
        }


        public int CreateEntity(GameObject entityPrefab)
        {
            Entity entity = entityPrefab.GetComponent<Entity>();

            if (RecycledEntityId != -1)
            {
                entity.Id = RecycledEntityId;
                RecycledEntityId = -1;
            }
            else
            {
                entity.Id = AvailableEntityId;
                AvailableEntityId++;
            }

            if (entity.Id > World.MAX_ENTITIES)
            {
                GameObject.Destroy(entityPrefab); //This could be a poolable object

                Debug.LogError($"Max entities exceeded. Tried to allocate {entityPrefab.name} to id {entity.Id}");

                return -1;
            }

            IComponent[] components = entityPrefab.GetComponents<IComponent>();

            for (int i = 0; i < components.Length; i++)
            {
                ComponentType componentType = components[i].GetComponentType();

                Components[(int)componentType].Add(entity.Id, components[i]);
            }

            Entities[entity.Id] = entity;

            Entities[entity.Id].Flags.SetFlag((int)EEntityFlags.Active);

            //TODO: Create and update entity archetypes for easier lookup and looping
            return entity.Id;
        }

        public void RemoveEntity(int entityID)
        {
            Entity entity = Entities[entityID];
            IComponent[] components = entity.GetComponents<IComponent>();

            for (int i = 0; i < components.Length; i++)
            {
                Components[(int)components[i].GetComponentType()].Remove(entityID);
            }

            RecycledEntityId = entityID;

            Entities[entityID].Id = -1;

            GameObject.Destroy(entity.gameObject); //NOTE: We can pool these entities to make sure we're being as efficient as possible with entity creation and removal
        }

        public static ComponentArray GetComponentArray(ComponentType componentType)
        {
            switch (componentType)
            {
                case ComponentType.Movement:
                    return new ComponentArray<MovementComponent>();
                case ComponentType.Health:
                    return new ComponentArray<HealthComponent>();
                case ComponentType.Ability:
                    return new ComponentArray<AbilityComponent>();
                case ComponentType.Damage:
                    return new ComponentArray<DamageComponent>();
                case ComponentType.Child:
                    return new ComponentArray<ChildComponent>();
                case ComponentType.Projectile:
                    return new ComponentArray<ProjectileComponent>();
                case ComponentType.BuffDebuff:
                    return new ComponentArray<BuffDebuffComponent>();
                case ComponentType.Throwable:
                    return new ComponentArray<ThrowableComponent>();
                case ComponentType.PlayerTag:
                    return new ComponentArray<PlayerTagComponent>(1);
                case ComponentType.DestroyOnHit:
                    return new ComponentArray<DestroyOnHitComponent>();
                case ComponentType.SpawnOnHit:
                    return new ComponentArray<SpawnOnHitComponent>();
                case ComponentType.DamageOnHit:
                    return new ComponentArray<DamageOnHitComponent>();
                case ComponentType.Collision:
                    return new ComponentArray<CollisionComponent>();
                case ComponentType.Attributes:
                    return new ComponentArray<AttributesComponent>();
                case ComponentType.BuffOnHit:
                    return new ComponentArray<BuffOnHitComponent>();
                case ComponentType.Interactable:
                    return new ComponentArray<InteractableComponent>();
                case ComponentType.Flammable:
                    return new ComponentArray<FlammableComponent>();
                case ComponentType.Inventory:
                    return new ComponentArray<InventoryComponent>();
                case ComponentType.Equipment:
                    return new ComponentArray<EquipmentComponent>();
                case ComponentType.Audio:
                    return new ComponentArray<AudioComponent>();
                case ComponentType.Max:
                    break;
            }

            return null;
        }
    }
}


