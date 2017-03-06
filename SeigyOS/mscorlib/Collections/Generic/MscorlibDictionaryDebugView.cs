using System.Diagnostics;

namespace System.Collections.Generic
{
    internal sealed class MscorlibDictionaryDebugView<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dict;

        public MscorlibDictionaryDebugView(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                __ThrowHelper.ThrowArgumentNullException(__ResourceName.ParamName_dictionary);
            _dict = dictionary;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<TKey, TValue>[] Items
        {
            get
            {
                KeyValuePair<TKey, TValue>[] items = new KeyValuePair<TKey, TValue>[_dict.Count];
                _dict.CopyTo(items, 0);
                return items;
            }
        }
    }
}