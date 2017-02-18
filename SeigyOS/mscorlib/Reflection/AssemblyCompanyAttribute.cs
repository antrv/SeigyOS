using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyCompanyAttribute: Attribute
    {
        private readonly string _company;

        public AssemblyCompanyAttribute(string company)
        {
            _company = company;
        }

        public string Company => _company;
    }
}