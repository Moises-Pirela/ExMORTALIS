using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;
using System.Reflection;
using System.Linq;

namespace Transendence.Core
{
    public class EntityContainer
    {
        public const int MAX_ENTITIES = 200;
        public Entity[] Entities;
        public ComponentArray[] Components;

        private int AvailableEntityId;
        private int RecycledEntityId = -1;

        public EntityContainer()
        {
            Entities = new Entity[MAX_ENTITIES];

            CreateComponentArrays();
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

        private void CreateComponentArrays()
        {
            var componentTypes = Assembly
                          .GetExecutingAssembly()
                          .GetTypes()
                          .Where(t => typeof(IComponent).IsAssignableFrom(t) && t != typeof(IComponent));

            Components = new ComponentArray[componentTypes.Count()];

            foreach (var type in componentTypes)
            {
                var componentInstance = (IComponent)Activator.CreateInstance(type);
                int componentType = (int)componentInstance.GetComponentType();

                var genericComponentArrayType = typeof(ComponentArray<>).MakeGenericType(type);

                Components[componentType] = (ComponentArray)Activator.CreateInstance(genericComponentArrayType, MAX_ENTITIES);

                Debug.Log($"Creating component array for {type.Name}");
            }
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

            if (entity.Id > MAX_ENTITIES)
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

            Debug.Log($"Created entity {entity.name} with id {entity.Id}");

            //TODO: Create and update entity archetypes for easier lookup and looping
            return entity.Id;
        }

        public void RemoveEntity(int entityID, bool destroy, float destroyTime)
        {
            Entity entity = Entities[entityID];
            IComponent[] components = entity.GetComponents<IComponent>();

            for (int i = 0; i < components.Length; i++)
            {
                Components[(int)components[i].GetComponentType()].Remove(entityID);
            }

            RecycledEntityId = entityID;
            
            Entities[entityID].Id = -1;

            if (destroy)
                GameObject.Destroy(entity.gameObject, destroyTime); //NOTE: We can pool these entities to make sure we're being as efficient as possible with entity creation and removal
        }
    }
}


