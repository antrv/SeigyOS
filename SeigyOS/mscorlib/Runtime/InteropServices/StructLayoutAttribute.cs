namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    [ComVisible(true)]
    public sealed class StructLayoutAttribute: Attribute
    {
        private readonly LayoutKind _val;

        public StructLayoutAttribute(LayoutKind layoutKind)
        {
            _val = layoutKind;
        }

        public StructLayoutAttribute(short layoutKind)
        {
            _val = (LayoutKind)layoutKind;
        }

        public LayoutKind Value => _val;
        public int Pack;
        public int Size;
        public CharSet CharSet;
    }
}