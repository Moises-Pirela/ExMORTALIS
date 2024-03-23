using UnityEngine;

namespace NL.Core.Configs
{
    [CreateAssetMenu(menuName = "Tools/Configs/Render", fileName = "RenderConfig")]
    public class RenderConfig : ScriptableObject
    {
        public string DisplayName;
        public string DisplayIcon;
    }
}
