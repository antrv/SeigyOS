namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    [ComVisible(true)]
    public sealed class UnmanagedFunctionPointerAttribute: Attribute
    {
        private readonly CallingConvention _callingConvention;

        public UnmanagedFunctionPointerAttribute(CallingConvention callingConvention)
        {
            _callingConvention = callingConvention;
        }

        public CallingConvention CallingConvention => _callingConvention;
        public CharSet CharSet;
        public bool BestFitMapping;
        public bool ThrowOnUnmappableChar;

        // This field is ignored and marshaling behaves as if it was true (for historical reasons).
        public bool SetLastError;
    }
}