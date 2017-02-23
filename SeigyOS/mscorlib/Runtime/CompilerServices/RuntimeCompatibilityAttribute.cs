namespace System.Runtime.CompilerServices
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class RuntimeCompatibilityAttribute: Attribute
    {
        private bool _wrapNonExceptionThrows;

        public RuntimeCompatibilityAttribute()
        {
        }

        public bool WrapNonExceptionThrows
        {
            get
            {
                return _wrapNonExceptionThrows;
            }
            set
            {
                _wrapNonExceptionThrows = value;
            }
        }
    }
}