namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)] 
    [ComVisible(true)]
    // ReSharper disable once InconsistentNaming
    public sealed class LCIDConversionAttribute : Attribute
    {
        private readonly int _val;
        public LCIDConversionAttribute(int lcid)
        {
            _val = lcid;
        }

        public int Value => _val;
    }
}