namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    [ComVisible(true)]
    public sealed class ComAliasNameAttribute: Attribute
    {
        private readonly string _val;

        public ComAliasNameAttribute(string alias)
        {
            _val = alias;
        }

        public string Value => _val;
    }
}