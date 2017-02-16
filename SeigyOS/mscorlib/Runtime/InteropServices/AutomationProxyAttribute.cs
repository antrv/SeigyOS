namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    [ComVisible(true)]
    public sealed class AutomationProxyAttribute: Attribute
    {
        private readonly bool _val;

        public AutomationProxyAttribute(bool val)
        {
            _val = val;
        }

        public bool Value => _val;
    }
}