namespace Transendence.Utilities
{
    public enum SystemAttributeType { Normal, PostProcess }
    public class SystemAttribute : System.Attribute
    {
        public SystemAttributeType SystemAttributeType;
        public SystemAttribute(SystemAttributeType type)
        {
            this.SystemAttributeType = type;
        }
    }
}


