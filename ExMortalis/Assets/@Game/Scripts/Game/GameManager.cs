using NL.Core;
using NL.Core.Configs;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace NL.ExMORTALIS.UI
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public World SimWorld;
        public WorldConfig WorldConfig;
        public UIManager UIManager;

        public void Awake()
        {
            Instance = this;
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
