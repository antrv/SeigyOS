namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    [ComVisible(true)]
    public sealed class ProgIdAttribute: Attribute
    {
        private readonly string _val;

        public ProgIdAttribute(string progId)
        {
            _val = progId;
        }

        public string Value => _val;
    }
}