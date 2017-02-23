using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface,
        AllowMultiple = true, Inherited = false)]
    [ComVisible(true)]
    public sealed class RequiredAttributeAttribute: Attribute
    {
        private readonly Type _requiredContract;

        public RequiredAttributeAttribute(Type requiredContract)
        {
            _requiredContract = requiredContract;
        }

        public Type RequiredContract => _requiredContract;
    }
}