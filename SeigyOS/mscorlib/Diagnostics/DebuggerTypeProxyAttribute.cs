using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    [ComVisible(true)]
    public sealed class DebuggerTypeProxyAttribute: Attribute
    {
        private readonly string _typeName;
        private string _targetName;
        private Type _target;

        public DebuggerTypeProxyAttribute(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            Contract.EndContractBlock();
            _typeName = type.AssemblyQualifiedName;
        }

        public DebuggerTypeProxyAttribute(string typeName)
        {
            _typeName = typeName;
        }

        public string ProxyTypeName => _typeName;

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