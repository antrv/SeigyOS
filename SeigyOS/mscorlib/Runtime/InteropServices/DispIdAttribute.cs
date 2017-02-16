namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Event, Inherited = false)]
    [ComVisible(true)]
    public sealed class DispIdAttribute: Attribute
    {
        private readonly int _val;

        public DispIdAttribute(int dispId)
        {
            _val = dispId;
        }

        public int Value => _val;
    }
}