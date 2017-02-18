using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyDescriptionAttribute: Attribute
    {
        private readonly string _description;

        public AssemblyDescriptionAttribute(string description)
        {
            _description = description;
        }

        public string Description => _description;
    }
}