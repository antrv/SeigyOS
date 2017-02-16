namespace System.Runtime.InteropServices
{
    // ReSharper disable once InconsistentNaming
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
    [Obsolete("This attribute is deprecated and will be removed in a future version.", false)]
    [ComVisible(true)]
    public sealed class IDispatchImplAttribute: Attribute
    {
        private readonly IDispatchImplType _val;

        public IDispatchImplAttribute(IDispatchImplType implType)
        {
            _val = implType;
        }

        public IDispatchImplAttribute(short implType)
        {
            _val = (IDispatchImplType)implType;
        }

        public IDispatchImplType Value => _val;
    }
}