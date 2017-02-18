using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyCopyrightAttribute: Attribute
    {
        private readonly string _copyright;

        public AssemblyCopyrightAttribute(string copyright)
        {
            _copyright = copyright;
        }

        public string Copyright => _copyright;
    }
}