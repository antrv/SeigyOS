namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate,
        Inherited = false, AllowMultiple = false)]
    public sealed class TypeForwardedFromAttribute: Attribute
    {
        private readonly string _assemblyFullName;

        public TypeForwardedFromAttribute(string assemblyFullName)
        {
            if (string.IsNullOrEmpty(assemblyFullName))
                throw new ArgumentNullException(nameof(assemblyFullName));
            _assemblyFullName = assemblyFullName;
        }

        public string AssemblyFullName => _assemblyFullName;
    }
}