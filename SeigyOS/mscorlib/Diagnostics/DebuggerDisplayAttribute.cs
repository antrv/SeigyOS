using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Delegate | AttributeTargets.Enum | 
                    AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Assembly, AllowMultiple = true)]
    [ComVisible(true)]
    public sealed class DebuggerDisplayAttribute: Attribute
    {
        private string _name;
        private readonly string _value;
        private string _type;
        private string _targetName;
        private Type _target;

        public DebuggerDisplayAttribute(string value)
        {
            _value = value ?? string.Empty;
            _name = string.Empty;
            _type = string.Empty;
        }

        public string Value => _value;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public Type Target
        {
            get
            {
                return _target;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                Contract.EndContractBlock();
                _targetName = value.AssemblyQualifiedName;
                _target = value;
            }
        }

        public string TargetTypeName
        {
            get
            {
                return _targetName;
            }
            set
            {
                _targetName = value;
            }
        }
    }
}