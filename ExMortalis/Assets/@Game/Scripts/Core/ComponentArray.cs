using System;
using Unity.Burst;

namespace Transendence.Core
{
    public abstract class ComponentArray
    {
        public abstract void Add(int entity, IComponent component);
        public abstract void UpdateComponents();

        public abstract void Remove(int entityID);
    }

    public class ComponentArray<Component> : ComponentArray where Component : IComponent
    {
        public Component[] Components;

        public ComponentArray(int size = World.MAX_ENTITIES)
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

        [BurstCompile]
        public override void UpdateComponents()
        {
            for (int i = 0; i < Components.Length; i++)
            {
                if (Components[i] == null) continue;

                Component component = Components[i];

                //if (component.OwningEntityId == -1) continue;

                //component.Tick();
            }
        }
    }
}
