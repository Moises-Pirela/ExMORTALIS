using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Transendence.Core;
using Transendence.Core.Configs;
using Transendence.Core.Postprocess;
using Transendence.Core.Systems;
using Transendence.Utilities;
using UnityEngine;

namespace Transendence.Core
{
    public class World
    {
        public static World Instance;
        public static int PLAYER_ENTITY_ID = -1;
        public EntityContainer EntityContainer;
        public WorldConfig WorldConfig;
        private List<BaseSystem> Systems = new List<BaseSystem>();
        private List<BaseSystem> FixedSystems = new List<BaseSystem>();
        private List<BaseSystem> PostProcessSystems = new List<BaseSystem>();
        private List<IPostProcessEvent> PostProcesses = new List<IPostProcessEvent>();

        public World(WorldConfig config)
        {
            Instance = this;

            EntityContainer = new EntityContainer();

            WorldConfig = config;

            LoadSystems();

        }

        public void AddPostProcessEvent(IPostProcessEvent processEvent)
        {
            PostProcesses.Add(processEvent);
        }

        public void Tick()
        {
            foreach (var system in Systems)
            {
                system.SystemUpdate(EntityContainer.Entities, EntityContainer.Components);
            }

            foreach (var system in PostProcessSystems)
            {
                system.SystemUpdate(EntityContainer.Entities, EntityContainer.Components, PostProcesses);
            }

            PostProcesses.Clear();
        }

        public void FixedTick()
        {
            foreach (var fixedSystem in FixedSystems)
            {
                fixedSystem.SystemUpdate(EntityContainer.Entities, EntityContainer.Components);
            }
        }

        private void LoadSystems()
        {
            var systemTypes = Assembly
                                .GetExecutingAssembly()
                                .GetTypes()
                                .Where(t => t.IsSubclassOf(typeof(BaseSystem)) && !t.IsAbstract)
                                .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<SystemAttribute>() })
                                .Where(t => t.Attribute != null)
                                .OrderBy(t => t.Attribute.Priority);

            foreach (var type in systemTypes)
            {
                var system = (BaseSystem)Activator.CreateInstance(type.Type);

                Debug.Log($"Loading system: {type.Type.Name}");

                switch (type.Attribute.SystemType)
                {
                    case SystemAttributeType.Normal:
                        Systems.Add(system);
                        break;
                    case SystemAttributeType.PostProcess:
                        PostProcessSystems.Add(system);
                        break;
                    case SystemAttributeType.Fixed:
                        FixedSystems.Add(system);
                        break;
                }
            }
        }
    }
}