namespace System.Runtime.InteropServices
{
    // ReSharper disable once InconsistentNaming
    [AttributeUsage(AttributeTargets.Method, Inherited = false)] 
    [ComVisible(true)]
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