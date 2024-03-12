using Transendence.Core;
using Transendence.Core.Configs;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Transendence.Game.UI
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public World SimWorld;
        public WorldConfig WorldConfig;
        public UIManager UIManager;

        public void Awake()
        {
            SimWorld = new World(WorldConfig);
            UIManager = new UIManager();
        }

        void Update()
        {
            SimWorld.Tick();
        }

        void FixedUpdate()
        {
            SimWorld.FixedTick();
        }
    }
}
