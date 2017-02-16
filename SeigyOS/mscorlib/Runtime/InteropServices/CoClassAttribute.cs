namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Interface, Inherited = false)]
    [ComVisible(true)]
    public sealed class CoClassAttribute: Attribute
    {
        private readonly Type _coClass;

        public CoClassAttribute(Type coClass)
        {
            _coClass = coClass;
        }

        public Type CoClass => _coClass;
    }
}