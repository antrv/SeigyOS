namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    [ComVisible(true)]
    public sealed class TypeLibVarAttribute: Attribute
    {
        private readonly TypeLibVarFlags _val;

        public TypeLibVarAttribute(TypeLibVarFlags flags)
        {
            _val = flags;
        }

        public TypeLibVarAttribute(short flags)
        {
            _val = (TypeLibVarFlags)flags;
        }

        public TypeLibVarFlags Value => _val;
    }
}