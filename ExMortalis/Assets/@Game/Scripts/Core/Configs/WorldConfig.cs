using UnityEngine;

namespace NL.Core.Configs
{
    [CreateAssetMenu(menuName = "Tools/Configs/World", fileName = "WorldConfig")]
    public class WorldConfig : BaseScriptableConfig
    {
        public EffectsLookupConfig EffectsLookupConfig;
        public BuffDebuffConfig[] BuffDebuffConfigs;
        public AbilityConfig[] AbilityConfigs;
        public InventoryItemConfig[] InventoryItemConfigs;
    }
}
