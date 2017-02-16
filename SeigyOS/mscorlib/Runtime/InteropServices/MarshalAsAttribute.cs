namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.ReturnValue, Inherited = false)]
    [ComVisible(true)]
    public sealed class MarshalAsAttribute: Attribute
    {
        private readonly UnmanagedType _val;

        public MarshalAsAttribute(UnmanagedType unmanagedType)
        {
            _val = unmanagedType;
        }

        public MarshalAsAttribute(short unmanagedType)
        {
            _val = (UnmanagedType)unmanagedType;
        }

        public UnmanagedType Value => _val;
        public VarEnum SafeArraySubType;
        public Type SafeArrayUserDefinedSubType;
        public int IidParameterIndex;
        public UnmanagedType ArraySubType;
        public short SizeParamIndex; // param index PI
        public int SizeConst; // constant C

        [ComVisible(true)]
        public string MarshalType; // Name of marshaler class

        [ComVisible(true)]
        public Type MarshalTypeRef; // Type of marshaler class

        public string MarshalCookie; // cookie to pass to marshaler
    }
}