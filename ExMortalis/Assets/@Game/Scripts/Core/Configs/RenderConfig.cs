using UnityEngine;

namespace Transendence.Core.Configs
{
    [CreateAssetMenu(menuName = "Tools/Configs/Render", fileName = "RenderConfig")]
    public class RenderConfig : ScriptableObject
    {
        public string DisplayName;
        public string DisplayIcon;
    }
}
