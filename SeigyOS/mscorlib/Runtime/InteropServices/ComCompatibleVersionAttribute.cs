namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class ComCompatibleVersionAttribute: Attribute
    {
        private readonly int _major;
        private readonly int _minor;
        private readonly int _build;
        private readonly int _revision;

        public ComCompatibleVersionAttribute(int major, int minor, int build, int revision)
        {
            _major = major;
            _minor = minor;
            _build = build;
            _revision = revision;
        }

        public int MajorVersion => _major;
        public int MinorVersion => _minor;
        public int BuildNumber => _build;
        public int RevisionNumber => _revision;
    }
}