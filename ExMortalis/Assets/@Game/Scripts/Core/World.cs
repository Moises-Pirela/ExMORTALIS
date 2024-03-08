using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using Transendence.Core.Configs;
using Transendence.Core.Postprocess;
using Transendence.Core.Systems;
using Transendence.Utilities;
using Unity.Burst;
using UnityEngine;
using UnityEngine.PlayerLoop;
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
        private List<BaseSystem> Systems = new List<BaseSystem>();
        private List<BaseSystem> FixedSystems = new List<BaseSystem>();
        private List<BaseSystem> PostProcessSystems = new List<BaseSystem>();
        private List<IPostProcessEvent> PostProcesses = new List<IPostProcessEvent>();

        public void Awake()
        {
            Instance = this;

            EntityContainer = new EntityContainer();

            LoadSystems();
        }

        public void AddPostProcessEvent(IPostProcessEvent processEvent)
        {
            PostProcesses.Add(processEvent);
        }

        private void Update()
        {
            foreach (var system in Systems)
            {
                system.Update(EntityContainer.Entities, EntityContainer.Components);
            }

            foreach (var system in PostProcessSystems)
            {
                system.Update(EntityContainer.Entities, EntityContainer.Components, PostProcesses);
            }

            PostProcesses.Clear();
        }

        private void FixedUpdate()
        {
            foreach (var fixedSystem in FixedSystems)
            {
                fixedSystem.Update(EntityContainer.Entities, EntityContainer.Components);
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