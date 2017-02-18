using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method |
                    AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Event |
                    AttributeTargets.Interface | AttributeTargets.Enum | AttributeTargets.Delegate, AllowMultiple = true, Inherited = false)]
    [ComVisible(true)]
    public sealed class ObfuscationAttribute: Attribute
    {
        private bool _strip = true;
        private bool _exclude = true;
        private bool _applyToMembers = true;
        private string _feature = "all";

        public bool StripAfterObfuscation
        {
            get
            {
                return _strip;
            }
            set
            {
                _strip = value;
            }
        }

        public bool Exclude
        {
            get
            {
                return _exclude;
            }
            set
            {
                _exclude = value;
            }
        }

        public bool ApplyToMembers
        {
            get
            {
                return _applyToMembers;
            }
            set
            {
                _applyToMembers = value;
            }
        }

        public string Feature
        {
            get
            {
                return _feature;
            }
            set
            {
                _feature = value;
            }
        }
    }
}