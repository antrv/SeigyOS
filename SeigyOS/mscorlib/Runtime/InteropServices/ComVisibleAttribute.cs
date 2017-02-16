namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class |
                    AttributeTargets.Struct | AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Field |
                    AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
    [ComVisible(true)]
    public sealed class ComVisibleAttribute: Attribute
    {
        private readonly bool _val;

        public ComVisibleAttribute(bool visibility)
        {
            _val = visibility;
        }

        public bool Value => _val;
    }
}