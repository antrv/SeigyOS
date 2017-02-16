namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Delegate,
        AllowMultiple = false, Inherited = false)]
    [ComVisible(false)]
    public sealed class TypeIdentifierAttribute: Attribute
    {
        private readonly string _scope;
        private readonly string _identifier;

        public TypeIdentifierAttribute()
        {
        }

        public TypeIdentifierAttribute(string scope, string identifier)
        {
            _scope = scope;
            _identifier = identifier;
        }

        public string Scope => _scope;
        public string Identifier => _identifier;
    }
}