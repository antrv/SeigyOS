using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyFileVersionAttribute: Attribute
    {
        private readonly string _version;

        public AssemblyFileVersionAttribute(string version)
        {
            if (version == null)
                throw new ArgumentNullException("version");
            Contract.EndContractBlock();
            _version = version;
        }

        public string Version => _version;
    }
}