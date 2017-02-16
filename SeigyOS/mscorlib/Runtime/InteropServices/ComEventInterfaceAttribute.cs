namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Interface, Inherited = false)]
    [ComVisible(true)]
    public sealed class ComEventInterfaceAttribute: Attribute
    {
        private readonly Type _sourceInterface;
        private readonly Type _eventProvider;

        public ComEventInterfaceAttribute(Type SourceInterface, Type EventProvider)
        {
            _sourceInterface = SourceInterface;
            _eventProvider = EventProvider;
        }

        public Type SourceInterface => _sourceInterface;
        public Type EventProvider => _eventProvider;
    }
}