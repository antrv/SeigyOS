using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyConfigurationAttribute: Attribute
    {
        private readonly string _configuration;

        public AssemblyConfigurationAttribute(string configuration)
        {
            _configuration = configuration;
        }

        public string Configuration => _configuration;
    }
}