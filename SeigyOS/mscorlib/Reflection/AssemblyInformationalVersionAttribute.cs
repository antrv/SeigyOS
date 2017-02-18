using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyInformationalVersionAttribute: Attribute
    {
        private readonly string _informationalVersion;

        public AssemblyInformationalVersionAttribute(string informationalVersion)
        {
            _informationalVersion = informationalVersion;
        }

        public string InformationalVersion => _informationalVersion;
    }
}