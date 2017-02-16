namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Enum | AttributeTargets.Struct, Inherited = false)]
    [ComVisible(true)]
    public sealed class TypeLibTypeAttribute: Attribute
    {
        private readonly TypeLibTypeFlags _val;

        public TypeLibTypeAttribute(TypeLibTypeFlags flags)
        {
            _val = flags;
        }

        public TypeLibTypeAttribute(short flags)
        {
            _val = (TypeLibTypeFlags)flags;
        }

        public TypeLibTypeFlags Value => _val;
    }
}