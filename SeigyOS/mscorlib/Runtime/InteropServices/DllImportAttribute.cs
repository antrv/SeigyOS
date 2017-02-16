namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    [ComVisible(true)]
    public sealed class DllImportAttribute: Attribute
    {
        private readonly string _val;

        public DllImportAttribute(string dllName)
        {
            _val = dllName;
        }

        public string Value => _val;
        public string EntryPoint;
        public CharSet CharSet;
        public bool SetLastError;
        public bool ExactSpelling;
        public bool PreserveSig;
        public CallingConvention CallingConvention;
        public bool BestFitMapping;
        public bool ThrowOnUnmappableChar;
    }
}