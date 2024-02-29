using UnityEngine;

namespace Transendence.Core.Configs
{
    [CreateAssetMenu(menuName = "Tools/Configs/Ability", fileName = "AbilityConfig")]
    public class AbilityConfig : BaseScriptableConfig
    {
        public float CooldownMs;
        public float Range;
        public float Radius;
        public GameObject AbilityPrefab;
    }
}
