namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
    [ComVisible(true)]
    public sealed class PrimaryInteropAssemblyAttribute: Attribute
    {
        private readonly int _major;
        private readonly int _minor;

        public PrimaryInteropAssemblyAttribute(int major, int minor)
        {
            _major = major;
            _minor = minor;
        }

        public int MajorVersion => _major;
        public int MinorVersion => _minor;
    }
}