using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyKeyNameAttribute: Attribute
    {
        private readonly string _keyName;

        public AssemblyKeyNameAttribute(string keyName)
        {
            _keyName = keyName;
        }

        public string KeyName => _keyName;
    }
}