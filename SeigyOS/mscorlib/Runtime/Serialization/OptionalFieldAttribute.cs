using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    [ComVisible(true)]
    public sealed class OptionalFieldAttribute: Attribute
    {
        private int _versionAdded = 1;

        public int VersionAdded
        {
            get
            {
                return _versionAdded;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentException(__Resources.GetResourceString("Serialization_OptionalFieldVersionValue"));
                Contract.EndContractBlock();
                _versionAdded = value;
            }
        }
    }
}