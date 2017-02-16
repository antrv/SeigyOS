namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Interface, Inherited = false)]
    [ComVisible(true)]
    public sealed class InterfaceTypeAttribute: Attribute
    {
        private readonly ComInterfaceType _val;

        public InterfaceTypeAttribute(ComInterfaceType interfaceType)
        {
            _val = interfaceType;
        }

        public InterfaceTypeAttribute(short interfaceType)
        {
            _val = (ComInterfaceType)interfaceType;
        }

        public ComInterfaceType Value => _val;
    }
}