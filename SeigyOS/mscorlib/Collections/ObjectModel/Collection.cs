using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Collections.ObjectModel
{
    [Serializable]
    [ComVisible(false)]
    [DebuggerTypeProxy(typeof(MscorlibCollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class Collection<T>: IList<T>, IList, IReadOnlyList<T>
    {
        private readonly IList<T> _items;

        [NonSerialized]
        private object _syncRoot;

        public Collection()
        {
            _items = new List<T>();
        }

        public Collection(IList<T> list)
        {
            if (list == null)
                __ThrowHelper.ThrowArgumentNullException(__ResourceName.ParamName_list);
            _items = list;
        }

        public int Count => _items.Count;
        protected IList<T> Items => _items;

        public T this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                if (_items.IsReadOnly)
                    __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
                if (index < 0 || index >= _items.Count)
                    __ThrowHelper.ThrowArgumentOutOfRangeException();
                SetItem(index, value);
            }
        }

        public void Add(T item)
        {
            if (_items.IsReadOnly)
                __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
            int index = _items.Count;
            InsertItem(index, item);
        }

        public void Clear()
        {
            if (_items.IsReadOnly)
                __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
            ClearItems();
        }

        public void CopyTo(T[] array, int index)
        {
            _items.CopyTo(array, index);
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (_items.IsReadOnly)
                __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
            if (index < 0 || index > _items.Count)
                __ThrowHelper.ThrowArgumentOutOfRangeException(__ResourceName.ParamName_index, __ResourceName.ArgumentOutOfRange_ListInsert);
            InsertItem(index, item);
        }

        public bool Remove(T item)
        {
            if (_items.IsReadOnly)
                __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
            int index = _items.IndexOf(item);
            if (index < 0)
                return false;
            RemoveItem(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            if (_items.IsReadOnly)
                __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
            if (index < 0 || index >= _items.Count)
                __ThrowHelper.ThrowArgumentOutOfRangeException();
            RemoveItem(index);
        }

        protected virtual void ClearItems()
        {
            _items.Clear();
        }

        protected virtual void InsertItem(int index, T item)
        {
            _items.Insert(index, item);
        }

        protected virtual void RemoveItem(int index)
        {
            _items.RemoveAt(index);
        }

        protected virtual void SetItem(int index, T item)
        {
            _items[index] = item;
        }

        bool ICollection<T>.IsReadOnly => _items.IsReadOnly;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
        }

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    ICollection c = _items as ICollection;
                    if (c != null)
                    {
                        _syncRoot = c.SyncRoot;
                    }
                    else
                    {
                        Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null);
                    }
                }
                return _syncRoot;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                __ThrowHelper.ThrowArgumentNullException(__ResourceName.ParamName_array);
            }

            if (array.Rank != 1)
            {
                __ThrowHelper.ThrowArgumentException(__ResourceName.Arg_RankMultiDimNotSupported);
            }

            if (array.GetLowerBound(0) != 0)
            {
                __ThrowHelper.ThrowArgumentException(__ResourceName.Arg_NonZeroLowerBound);
            }

            if (index < 0)
            {
                __ThrowHelper.ThrowArgumentOutOfRangeException(__ResourceName.ParamName_index, __ResourceName.ArgumentOutOfRange_NeedNonNegNum);
            }

            if (array.Length - index < Count)
            {
                __ThrowHelper.ThrowArgumentException(__ResourceName.Arg_ArrayPlusOffTooSmall);
            }

            T[] tArray = array as T[];
            if (tArray != null)
            {
                _items.CopyTo(tArray, index);
            }
            else
            {
                //
                // Catch the obvious case assignment will fail.
                // We can found all possible problems by doing the check though.
                // For example, if the element type of the Array is derived from T,
                // we can't figure out if we can successfully copy the element beforehand.
                //
                Type targetType = array.GetType().GetElementType();
                Type sourceType = typeof(T);
                if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType)))
                {
                    __ThrowHelper.ThrowArgumentException(__ResourceName.Argument_InvalidArrayType);
                }

                //
                // We can't cast array of value type to object[], so we don't support 
                // widening of primitive types here.
                //
                object[] objects = array as object[];
                if (objects == null)
                {
                    __ThrowHelper.ThrowArgumentException(__ResourceName.Argument_InvalidArrayType);
                }

                int count = _items.Count;
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        objects[index++] = _items[i];
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    __ThrowHelper.ThrowArgumentException(__ResourceName.Argument_InvalidArrayType);
                }
            }
        }

        object IList.this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                __ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, __ResourceName.ParamName_value);

                try
                {
                    this[index] = (T)value;
                }
                catch (InvalidCastException)
                {
                    __ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
                }

            }
        }

        bool IList.IsReadOnly => _items.IsReadOnly;

        bool IList.IsFixedSize
        {
            get
            {
                IList list = _items as IList;
                if (list != null)
                    return list.IsFixedSize;
                return _items.IsReadOnly;
            }
        }

        int IList.Add(object value)
        {
            if (_items.IsReadOnly)
                __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
            __ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, __ResourceName.ParamName_value);

            try
            {
                Add((T)value);
            }
            catch (InvalidCastException)
            {
                __ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
            }

            return this.Count - 1;
        }

        bool IList.Contains(object value)
        {
            if (IsCompatibleObject(value))
                return Contains((T)value);
            return false;
        }

        int IList.IndexOf(object value)
        {
            if (IsCompatibleObject(value))
            {
                return IndexOf((T)value);
            }
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            if (_items.IsReadOnly)
                __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
            __ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, __ResourceName.ParamName_value);

            try
            {
                Insert(index, (T)value);
            }
            catch (InvalidCastException)
            {
                __ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
            }

        }

        void IList.Remove(object value)
        {
            if (_items.IsReadOnly)
                __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
            if (IsCompatibleObject(value))
                Remove((T)value);
        }

        private static bool IsCompatibleObject(object value)
        {
            return (value is T) || (value == null && default(T) == null);
        }
    }
    [Serializable]
    [DebuggerTypeProxy(typeof(Mscorlib_DictionaryDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> m_dictionary;
        [NonSerialized]
        private Object m_syncRoot;
        [NonSerialized]
        private KeyCollection m_keys;
        [NonSerialized]
        private ValueCollection m_values;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary) {
            if (dictionary == null) {
                throw new ArgumentNullException("dictionary");
            }
            Contract.EndContractBlock();
            m_dictionary = dictionary;
        }

        protected IDictionary<TKey, TValue> Dictionary {
            get { return m_dictionary; }
        }

        public KeyCollection Keys {
            get {
                Contract.Ensures(Contract.Result<KeyCollection>() != null);
                if (m_keys == null) {
                    m_keys = new KeyCollection(m_dictionary.Keys);
                }
                return m_keys;
            }
        }

        public ValueCollection Values {
            get {
                Contract.Ensures(Contract.Result<ValueCollection>() != null);
                if (m_values == null) {
                    m_values = new ValueCollection(m_dictionary.Values);
                }
                return m_values;
            }
        }

        #region IDictionary<TKey, TValue> Members

        public bool ContainsKey(TKey key) {
            return m_dictionary.ContainsKey(key);
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys {
            get {
                return Keys;
            }
        }

        public bool TryGetValue(TKey key, out TValue value) {
            return m_dictionary.TryGetValue(key, out value);
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values {
            get {
                return Values;
            }
        }

        public TValue this[TKey key] {
            get {
                return m_dictionary[key];
            }
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) {
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key) {
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            return false;
        }

        TValue IDictionary<TKey, TValue>.this[TKey key] {
            get {
                return m_dictionary[key];
            }
            set {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey, TValue>> Members

        public int Count {
            get { return m_dictionary.Count; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) {
            return m_dictionary.Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            m_dictionary.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly {
            get { return true; }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) {
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear() {
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) {
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            return false;
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey, TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return m_dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((IEnumerable)m_dictionary).GetEnumerator();
        }

        #endregion

        #region IDictionary Members

        private static bool IsCompatibleKey(object key) {
            if (key == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
            }
            return key is TKey;
        }

        void IDictionary.Add(object key, object value) {
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
        }

        void IDictionary.Clear() {
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
        }

        bool IDictionary.Contains(object key) {
            return IsCompatibleKey(key) && ContainsKey((TKey)key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator() {
            IDictionary d = m_dictionary as IDictionary;
            if (d != null) {
                return d.GetEnumerator();
            }
            return new DictionaryEnumerator(m_dictionary);
        }

        bool IDictionary.IsFixedSize {
            get { return true; }
        }

        bool IDictionary.IsReadOnly {
            get { return true; }
        }

        ICollection IDictionary.Keys {
            get {
                return Keys;
            }
        }

        void IDictionary.Remove(object key) {
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
        }

        ICollection IDictionary.Values {
            get {
                return Values;
            }
        }

        object IDictionary.this[object key] {
            get {
                if (IsCompatibleKey(key)) {
                    return this[(TKey)key];
                }
                return null;
            }
            set {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            }
        }

        void ICollection.CopyTo(Array array, int index) {
            if (array == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            }

            if (array.Rank != 1) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
            }

            if (array.GetLowerBound(0) != 0) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
            }

            if (index < 0 || index > array.Length) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }

            if (array.Length - index < Count) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
            }

            KeyValuePair<TKey, TValue>[] pairs = array as KeyValuePair<TKey, TValue>[];
            if (pairs != null) {
                m_dictionary.CopyTo(pairs, index);
            }
            else {
                DictionaryEntry[] dictEntryArray = array as DictionaryEntry[];
                if (dictEntryArray != null) {
                    foreach (var item in m_dictionary) {
                        dictEntryArray[index++] = new DictionaryEntry(item.Key, item.Value);
                    }
                }
                else {
                    object[] objects = array as object[];
                    if (objects == null) {
                        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                    }

                    try {
                        foreach (var item in m_dictionary) {
                            objects[index++] = new KeyValuePair<TKey, TValue>(item.Key, item.Value);
                        }
                    }
                    catch (ArrayTypeMismatchException) {
                        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                    }
                }
            }
        }

        bool ICollection.IsSynchronized {
            get { return false; }
        }

        object ICollection.SyncRoot {
            get {
                if (m_syncRoot == null) {
                    ICollection c = m_dictionary as ICollection;
                    if (c != null) {
                        m_syncRoot = c.SyncRoot;
                    }
                    else {
                        System.Threading.Interlocked.CompareExchange<Object>(ref m_syncRoot, new Object(), null);
                    }
                }
                return m_syncRoot;
            }
        }

        [Serializable]
        private struct DictionaryEnumerator : IDictionaryEnumerator {
            private readonly IDictionary<TKey, TValue> m_dictionary;
            private IEnumerator<KeyValuePair<TKey, TValue>> m_enumerator;

            public DictionaryEnumerator(IDictionary<TKey, TValue> dictionary) {
                m_dictionary = dictionary;
                m_enumerator = m_dictionary.GetEnumerator();
            }

            public DictionaryEntry Entry {
                get { return new DictionaryEntry(m_enumerator.Current.Key, m_enumerator.Current.Value); }
            }

            public object Key {
                get { return m_enumerator.Current.Key; }
            }

            public object Value {
                get { return m_enumerator.Current.Value; }
            }

            public object Current {
                get { return Entry; }
            }

            public bool MoveNext() {
                return m_enumerator.MoveNext();
            }

            public void Reset() {
                m_enumerator.Reset();
            }
        }

        #endregion

        #region IReadOnlyDictionary members

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys {
            get {
                return Keys;
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values {
            get {
                return Values;
            }
        }

        #endregion IReadOnlyDictionary members

        [DebuggerTypeProxy(typeof(Mscorlib_CollectionDebugView<>))]
        [DebuggerDisplay("Count = {Count}")]
        [Serializable]
        public sealed class KeyCollection : ICollection<TKey>, ICollection, IReadOnlyCollection<TKey> {
            private readonly ICollection<TKey> m_collection;
            [NonSerialized]
            private Object m_syncRoot;

            internal KeyCollection(ICollection<TKey> collection)
            {
                if (collection == null) {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
                }
                m_collection = collection;
            }

            #region ICollection<T> Members

            void ICollection<TKey>.Add(TKey item)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            }

            void ICollection<TKey>.Clear()
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            }

            bool ICollection<TKey>.Contains(TKey item)
            {
                return m_collection.Contains(item);
            }

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                m_collection.CopyTo(array, arrayIndex);
            }

            public int Count {
                get { return m_collection.Count; }
            }

            bool ICollection<TKey>.IsReadOnly {
                get { return true; }
            }

            bool ICollection<TKey>.Remove(TKey item)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
                return false;
            }

            #endregion

            #region IEnumerable<T> Members

            public IEnumerator<TKey> GetEnumerator()
            {
                return m_collection.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return ((IEnumerable)m_collection).GetEnumerator();
            }

            #endregion

            #region ICollection Members

            void ICollection.CopyTo(Array array, int index) {
                ReadOnlyDictionaryHelpers.CopyToNonGenericICollectionHelper<TKey>(m_collection, array, index);
            }

            bool ICollection.IsSynchronized {
                get { return false; }
            }

            object ICollection.SyncRoot {
                get {
                    if (m_syncRoot == null) {
                        ICollection c = m_collection as ICollection;
                        if (c != null) {
                            m_syncRoot = c.SyncRoot;
                        }
                        else {
                            System.Threading.Interlocked.CompareExchange<Object>(ref m_syncRoot, new Object(), null);
                        }
                    }
                    return m_syncRoot;
                }
            }

            #endregion
        }

        [DebuggerTypeProxy(typeof(Mscorlib_CollectionDebugView<>))]
        [DebuggerDisplay("Count = {Count}")]
        [Serializable]
        public sealed class ValueCollection : ICollection<TValue>, ICollection, IReadOnlyCollection<TValue> {
            private readonly ICollection<TValue> m_collection;
            [NonSerialized]
            private Object m_syncRoot;

            internal ValueCollection(ICollection<TValue> collection)
            {
                if (collection == null) {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
                }
                m_collection = collection;
            }

            #region ICollection<T> Members

            void ICollection<TValue>.Add(TValue item)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            }

            void ICollection<TValue>.Clear()
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            }

            bool ICollection<TValue>.Contains(TValue item)
            {
                return m_collection.Contains(item);
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                m_collection.CopyTo(array, arrayIndex);
            }

            public int Count {
                get { return m_collection.Count; }
            }

            bool ICollection<TValue>.IsReadOnly {
                get { return true; }
            }

            bool ICollection<TValue>.Remove(TValue item)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
                return false;
            }

            #endregion

            #region IEnumerable<T> Members

            public IEnumerator<TValue> GetEnumerator()
            {
                return m_collection.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return ((IEnumerable)m_collection).GetEnumerator();
            }

            #endregion

            #region ICollection Members

            void ICollection.CopyTo(Array array, int index) {
                ReadOnlyDictionaryHelpers.CopyToNonGenericICollectionHelper<TValue>(m_collection, array, index);
            }

            bool ICollection.IsSynchronized {
                get { return false; }
            }

            object ICollection.SyncRoot {
                get {
                    if (m_syncRoot == null) {
                        ICollection c = m_collection as ICollection;
                        if (c != null) {
                            m_syncRoot = c.SyncRoot;
                        }
                        else {
                            System.Threading.Interlocked.CompareExchange<Object>(ref m_syncRoot, new Object(), null);
                        }
                    }
                    return m_syncRoot;
                }
            }

            #endregion ICollection Members
        }
    }

    // To share code when possible, use a non-generic class to get rid of irrelevant type parameters.
    internal static class ReadOnlyDictionaryHelpers
    {
        #region Helper method for our KeyCollection and ValueCollection

        // Abstracted away to avoid redundant implementations.
        internal static void CopyToNonGenericICollectionHelper<T>(ICollection<T> collection, Array array, int index)
        {
            if (array == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            }

            if (array.Rank != 1) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
            }

            if (array.GetLowerBound(0) != 0) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
            }

            if (index < 0) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }

            if (array.Length - index < collection.Count) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
            }

            // Easy out if the ICollection<T> implements the non-generic ICollection
            ICollection nonGenericCollection = collection as ICollection;
            if (nonGenericCollection != null) {
                nonGenericCollection.CopyTo(array, index);
                return;
            }

            T[] items = array as T[];
            if (items != null) {
                collection.CopyTo(items, index);
            }
            else {
                //
                // Catch the obvious case assignment will fail.
                // We can found all possible problems by doing the check though.
                // For example, if the element type of the Array is derived from T,
                // we can't figure out if we can successfully copy the element beforehand.
                //
                Type targetType = array.GetType().GetElementType();
                Type sourceType = typeof(T);
                if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType))) {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                }

                //
                // We can't cast array of value type to object[], so we don't support 
                // widening of primitive types here.
                //
                object[] objects = array as object[];
                if (objects == null) {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                }

                try {
                    foreach (var item in collection) {
                        objects[index++] = item;
                    }
                }
                catch (ArrayTypeMismatchException) {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                }
            }
        }

        #endregion Helper method for our KeyCollection and ValueCollection
    }
}