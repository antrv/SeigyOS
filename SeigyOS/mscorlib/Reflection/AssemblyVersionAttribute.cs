using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyVersionAttribute: Attribute
    {
        private readonly string _version;

        public AssemblyVersionAttribute(string version)
        {
            _version = version;
        }

        public string Version => _version;
    }
}