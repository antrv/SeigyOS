using System.Diagnostics;

namespace System.Collections.Generic
{
    internal sealed class MscorlibKeyedCollectionDebugView<TKey, TValue>
    {
        private readonly KeyedCollection<TKey, TValue> _kc;

        public MscorlibKeyedCollectionDebugView(KeyedCollection<TKey, TValue> keyedCollection)
        {
            if (keyedCollection == null)
                __ThrowHelper.ThrowArgumentNullException(__ResourceName.ParamName_keyedCollection);
            _kc = keyedCollection;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public TValue[] Items
        {
            get
            {
                TValue[] items = new TValue[_kc.Count];
                _kc.CopyTo(items, 0);
                return items;
            }
        }
    }
}