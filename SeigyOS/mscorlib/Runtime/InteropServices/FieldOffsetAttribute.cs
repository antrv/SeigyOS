namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    [ComVisible(true)]
    public sealed class FieldOffsetAttribute: Attribute
    {
        private readonly int _val;

        public FieldOffsetAttribute(int offset)
        {
            _val = offset;
        }

        public int Value => _val;
    }
}