using System.Runtime.InteropServices;

namespace System.Resources
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    [ComVisible(true)]
    public sealed class SatelliteContractVersionAttribute: Attribute
    {
        private readonly string _version;

        public SatelliteContractVersionAttribute(string version)
        {
            if (version == null)
                throw new ArgumentNullException("version");
            Contract.EndContractBlock();
            _version = version;
        }

        public string Version => _version;
    }
}