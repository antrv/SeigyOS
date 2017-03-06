using System.Diagnostics;

namespace System.Collections.Generic
{
    internal sealed class MscorlibDictionaryKeyCollectionDebugView<TKey, TValue>
    {
        private readonly ICollection<TKey> _collection;

        public MscorlibDictionaryKeyCollectionDebugView(ICollection<TKey> collection)
        {
            if (collection == null)
                __ThrowHelper.ThrowArgumentNullException(__ResourceName.ParamName_collection);
            _collection = collection;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public TKey[] Items
        {
            get
            {
                TKey[] items = new TKey[_collection.Count];
                _collection.CopyTo(items, 0);
                return items;
            }
        }
    }
}