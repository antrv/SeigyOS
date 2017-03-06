using System.Diagnostics;

namespace System.Collections.Generic
{
    internal sealed class MscorlibDictionaryValueCollectionDebugView<TKey, TValue>
    {
        private readonly ICollection<TValue> _collection;

        public MscorlibDictionaryValueCollectionDebugView(ICollection<TValue> collection)
        {
            if (collection == null)
                __ThrowHelper.ThrowArgumentNullException(__ResourceName.ParamName_collection);
            _collection = collection;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public TValue[] Items
        {
            get
            {
                TValue[] items = new TValue[_collection.Count];
                _collection.CopyTo(items, 0);
                return items;
            }
        }
    }
}