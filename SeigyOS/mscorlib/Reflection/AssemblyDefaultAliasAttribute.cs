using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyDefaultAliasAttribute: Attribute
    {
        private readonly string _defaultAlias;

        public AssemblyDefaultAliasAttribute(string defaultAlias)
        {
            _defaultAlias = defaultAlias;
        }

        public string DefaultAlias => _defaultAlias;
    }
}