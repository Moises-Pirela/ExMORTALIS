using System;

namespace NL.Utilities
{
    public enum SystemAttributeType { Normal, PostProcess, Fixed }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SystemAttribute : System.Attribute
    {
        public SystemAttributeType SystemType { get; private set; }
        public int Priority { get; private set; }
        public SystemAttribute(SystemAttributeType type, int priority = 0)
        {
            SystemType = type;
            Priority = priority;
        }
    }
}


