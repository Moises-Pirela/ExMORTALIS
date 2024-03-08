using System;

namespace Transendence.Utilities
{
    public enum SystemAttributeType { Normal, PostProcess, Fixed }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SystemAttribute : System.Attribute
    {
        public SystemAttributeType SystemType { get; private set; }
        public int Priority { get; private set; }
        public SystemAttribute(SystemAttributeType type)
        {
            SystemType = type;
        }
    }
}


