using System;
using Unity.Burst;
using UnityEngine;

namespace NL.Core
{
    public abstract class ComponentArray
    {
        public abstract void Add(int entity, IComponent component);

        public abstract void Remove(int entityID);
    }

    public class ComponentArray<Component> : ComponentArray where Component : IComponent
    {
        public Component[] Components;

        public ComponentArray(int size = EntityContainer.MAX_ENTITIES) 
        {
            Components = new Component[size];
        }

        public override void Add(int entityId, IComponent component)
        {
            Components[entityId] = (Component)component;
        }

        public override void Remove(int entityID)
        {
            Components[entityID] = default(Component);
        }
    }
}
