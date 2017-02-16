namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Enum |
                    AttributeTargets.Struct | AttributeTargets.Delegate, Inherited = false)]
    [ComVisible(true)]
    public sealed class GuidAttribute: Attribute
    {
        private readonly string _val;

        public GuidAttribute(string guid)
        {
            _val = guid;
        }

        public string Value => _val;
    }
}