namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, Inherited = false)]
    [ComVisible(true)]
    public sealed class ClassInterfaceAttribute: Attribute
    {
        private readonly ClassInterfaceType _val;

        public ClassInterfaceAttribute(ClassInterfaceType classInterfaceType)
        {
            _val = classInterfaceType;
        }

        public ClassInterfaceAttribute(short classInterfaceType)
        {
            _val = (ClassInterfaceType)classInterfaceType;
        }

        public ClassInterfaceType Value => _val;
    }
}