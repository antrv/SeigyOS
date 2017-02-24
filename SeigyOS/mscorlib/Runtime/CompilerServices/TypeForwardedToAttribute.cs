namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed class TypeForwardedToAttribute: Attribute
    {
        private readonly Type _destination;

        public TypeForwardedToAttribute(Type destination)
        {
            _destination = destination;
        }

        public Type Destination => _destination;
    }
}