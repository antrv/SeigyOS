using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyProductAttribute: Attribute
    {
        private readonly string _product;

        public AssemblyProductAttribute(string product)
        {
            _product = product;
        }

        public string Product => _product;
    }
}