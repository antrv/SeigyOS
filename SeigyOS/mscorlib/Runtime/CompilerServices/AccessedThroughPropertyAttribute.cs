using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Field)]
    [ComVisible(true)]
    public sealed class AccessedThroughPropertyAttribute: Attribute
    {
        private readonly string _propertyName;

        public AccessedThroughPropertyAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }

        public string PropertyName => _propertyName;
    }
}