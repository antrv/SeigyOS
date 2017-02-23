namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public sealed class FixedBufferAttribute: Attribute
    {
        private readonly Type _elementType;
        private readonly int _length;

        public FixedBufferAttribute(Type elementType, int length)
        {
            _elementType = elementType;
            _length = length;
        }

        public Type ElementType => _elementType;
        public int Length => _length;
    }
}