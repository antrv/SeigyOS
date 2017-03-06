using System.Diagnostics;

namespace System.Collections.Generic
{
    internal sealed class MscorlibCollectionDebugView<T>
    {
        private readonly ICollection<T> _collection;

        public MscorlibCollectionDebugView(ICollection<T> collection)
        {
            if (collection == null)
                __ThrowHelper.ThrowArgumentNullException(__ResourceName.ParamName_collection);
            _collection = collection;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                T[] items = new T[_collection.Count];
                _collection.CopyTo(items, 0);
                return items;
            }
        }
    }
}