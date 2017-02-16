using System.Runtime.InteropServices;

namespace System
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum |
                    AttributeTargets.Interface | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property |
                    AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Delegate, Inherited = false)]
    [ComVisible(true)]
    public sealed class ObsoleteAttribute: Attribute
    {
        private readonly string _message;
        private readonly bool _error;

        public ObsoleteAttribute()
        {
            _message = null;
            _error = false;
        }

        public ObsoleteAttribute(string message)
        {
            _message = message;
            _error = false;
        }

        public ObsoleteAttribute(string message, bool error)
        {
            _message = message;
            _error = error;
        }

        public string Message => _message;
        public bool IsError => _error;
    }
}