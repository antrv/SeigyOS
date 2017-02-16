namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class TypeLibVersionAttribute: Attribute
    {
        private readonly int _major;
        private readonly int _minor;

        public TypeLibVersionAttribute(int major, int minor)
        {
            _major = major;
            _minor = minor;
        }

        public int MajorVersion => _major;
        public int MinorVersion => _minor;
    }
}