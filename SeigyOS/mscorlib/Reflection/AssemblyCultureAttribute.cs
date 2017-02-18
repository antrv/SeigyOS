using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyCultureAttribute: Attribute
    {
        private readonly string _culture;

        public AssemblyCultureAttribute(string culture)
        {
            _culture = culture;
        }

        public string Culture => _culture;
    }
}