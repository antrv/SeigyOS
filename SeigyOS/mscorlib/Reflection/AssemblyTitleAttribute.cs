using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyTitleAttribute: Attribute
    {
        private readonly string _title;

        public AssemblyTitleAttribute(string title)
        {
            _title = title;
        }

        public string Title => _title;
    }
}