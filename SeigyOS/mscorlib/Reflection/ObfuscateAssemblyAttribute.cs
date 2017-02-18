using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    [ComVisible(true)]
    public sealed class ObfuscateAssemblyAttribute: Attribute
    {
        private readonly bool _assemblyIsPrivate;
        private bool _strip = true;

        public ObfuscateAssemblyAttribute(bool assemblyIsPrivate)
        {
            _assemblyIsPrivate = assemblyIsPrivate;
        }

        public bool AssemblyIsPrivate => _assemblyIsPrivate;

        public bool StripAfterObfuscation
        {
            get
            {
                return _strip;
            }
            set
            {
                _strip = value;
            }
        }
    }
}