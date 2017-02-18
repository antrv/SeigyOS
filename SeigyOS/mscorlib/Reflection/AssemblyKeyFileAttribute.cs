using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyKeyFileAttribute: Attribute
    {
        private readonly string _keyFile;

        public AssemblyKeyFileAttribute(string keyFile)
        {
            _keyFile = keyFile;
        }

        public string KeyFile => _keyFile;
    }
}