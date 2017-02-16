namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    [ComVisible(true)]
    public sealed class TypeLibFuncAttribute: Attribute
    {
        private readonly TypeLibFuncFlags _val;

        public TypeLibFuncAttribute(TypeLibFuncFlags flags)
        {
            _val = flags;
        }

        public TypeLibFuncAttribute(short flags)
        {
            _val = (TypeLibFuncFlags)flags;
        }

        public TypeLibFuncFlags Value => _val;
    }
}