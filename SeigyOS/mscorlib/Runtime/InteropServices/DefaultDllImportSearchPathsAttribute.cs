namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Method, AllowMultiple = false)]
    [ComVisible(false)]
    public sealed class DefaultDllImportSearchPathsAttribute: Attribute
    {
        private readonly DllImportSearchPath _paths;

        public DefaultDllImportSearchPathsAttribute(DllImportSearchPath paths)
        {
            _paths = paths;
        }

        public DllImportSearchPath Paths => _paths;
    }
}