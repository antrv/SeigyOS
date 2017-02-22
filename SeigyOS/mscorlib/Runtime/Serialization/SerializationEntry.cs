using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
    [ComVisible(true)]
    public struct SerializationEntry
    {
        private readonly Type _type;
        private readonly object _value;
        private readonly string _name;

        internal SerializationEntry(string entryName, object entryValue, Type entryType)
        {
            _value = entryValue;
            _name = entryName;
            _type = entryType;
        }

        public object Value => _value;
        public string Name => _name;
        public Type ObjectType => _type;
    }
}