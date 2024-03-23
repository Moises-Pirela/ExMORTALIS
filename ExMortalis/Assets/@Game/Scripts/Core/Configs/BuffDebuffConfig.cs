using NL.Core;
using UnityEngine;

namespace NL.Core.Configs
{
    [CreateAssetMenu(menuName = "Tools/Configs/BuffDebuff", fileName = "BuffDebuff")]
    public class BuffDebuffConfig : BaseScriptableConfig
    {
        public BuffDebuffType Type;
        public BuffApplicationType ApplicationType;
        public BuffDuplicationType DuplicationType;

        public float BaseMagnitude;
        public float FlatMagnitude;
        [Range(0f, 1f)] public float PercentageMagnitude;
        [Range(0f, 10f)] public float DurationSec;
        [Range(1, 3)] public int StacksToApply;

        [Range(0.1f, 10f)] public float MaxDuration;
        [Range(1f, 3f)] public int MaxStacks;
        [Range(0.1f, 1f)] public float MaxPerc;
    }
}
