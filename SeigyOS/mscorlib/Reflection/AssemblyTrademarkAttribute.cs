using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyTrademarkAttribute: Attribute
    {
        private readonly string _trademark;

        public AssemblyTrademarkAttribute(string trademark)
        {
            _trademark = trademark;
        }

        public string Trademark => _trademark;
    }
}