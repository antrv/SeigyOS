namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    [ComVisible(false)]
    public sealed class ManagedToNativeComInteropStubAttribute: Attribute
    {
        private readonly Type _classType;
        private readonly string _methodName;

        public ManagedToNativeComInteropStubAttribute(Type classType, String methodName)
        {
            _classType = classType;
            _methodName = methodName;
        }

        public Type ClassType => _classType;
        public string MethodName => _methodName;
    }
}