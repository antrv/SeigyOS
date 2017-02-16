namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Module, Inherited = false)]
    [ComVisible(true)]
    public sealed class DefaultCharSetAttribute: Attribute
    {
        private readonly CharSet _charSet;

        public DefaultCharSetAttribute(CharSet charSet)
        {
            _charSet = charSet;
        }

        public CharSet CharSet => _charSet;
    }
}