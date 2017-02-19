using System.Runtime.InteropServices;

namespace System.Collections
{
    [ComVisible(true)]
    public interface IDictionary: ICollection
    {
        object this[object key] { get; set; }
        ICollection Keys { get; }
        ICollection Values { get; }
        bool Contains(object key);
        void Add(object key, object value);
        void Clear();
        bool IsReadOnly { get; }
        bool IsFixedSize { get; }
        new IDictionaryEnumerator GetEnumerator();
        void Remove(object key);
    }
}