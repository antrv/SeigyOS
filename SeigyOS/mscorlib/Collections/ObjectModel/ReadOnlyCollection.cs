using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Collections.ObjectModel
{
    [Serializable]
    [ComVisible(false)]
    [DebuggerTypeProxy(typeof(MscorlibCollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class ReadOnlyCollection<T>: IList<T>, IList, IReadOnlyList<T>
    {
        private readonly IList<T> _list;

        [NonSerialized]
        private object _syncRoot;

        public ReadOnlyCollection(IList<T> list)
        {
            if (list == null)
                __ThrowHelper.ThrowArgumentNullException(__ResourceName.ParamName_list);
            _list = list;
        }

        public int Count => _list.Count;
        public T this[int index] => _list[index];

        public bool Contains(T value)
        {
            return _list.Contains(value);
        }

        public void CopyTo(T[] array, int index)
        {
            _list.CopyTo(array, index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public int IndexOf(T value)
        {
            return _list.IndexOf(value);
        }

        protected IList<T> Items => _list;
        bool ICollection<T>.IsReadOnly => true;

        T IList<T>.this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
            }
        }

        void ICollection<T>.Add(T value)
        {
            __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
        }

        void ICollection<T>.Clear()
        {
            __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
        }

        void IList<T>.Insert(int index, T value)
        {
            __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
        }

        bool ICollection<T>.Remove(T value)
        {
            __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
            return false;
        }

        void IList<T>.RemoveAt(int index)
        {
            __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    ICollection c = _list as ICollection;
                    if (c != null)
                        _syncRoot = c.SyncRoot;
                    else
                        Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null);
                }
                return _syncRoot;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                __ThrowHelper.ThrowArgumentNullException(__ResourceName.ParamName_array);
            if (array.Rank != 1)
                __ThrowHelper.ThrowArgumentException(__ResourceName.Arg_RankMultiDimNotSupported);
            if (array.GetLowerBound(0) != 0)
                __ThrowHelper.ThrowArgumentException(__ResourceName.Arg_NonZeroLowerBound);
            if (index < 0)
                __ThrowHelper.ThrowArgumentOutOfRangeException(__ResourceName.ParamName_arrayIndex, __ResourceName.ArgumentOutOfRange_NeedNonNegNum);
            if (array.Length - index < Count)
                __ThrowHelper.ThrowArgumentException(__ResourceName.Arg_ArrayPlusOffTooSmall);

            T[] items = array as T[];
            if (items != null)
            {
                _list.CopyTo(items, index);
            }
            else
            {
                Type targetType = array.GetType().GetElementType();
                Type sourceType = typeof(T);
                if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType)))
                    __ThrowHelper.ThrowArgumentException(__ResourceName.Argument_InvalidArrayType);

                object[] objects = array as object[];
                if (objects == null)
                    __ThrowHelper.ThrowArgumentException(__ResourceName.Argument_InvalidArrayType);

                int count = _list.Count;
                try
                {
                    for (int i = 0; i < count; i++)
                        objects[index++] = _list[i];
                }
                catch (ArrayTypeMismatchException)
                {
                    __ThrowHelper.ThrowArgumentException(__ResourceName.Argument_InvalidArrayType);
                }
            }
        }

        bool IList.IsFixedSize => true;

        bool IList.IsReadOnly => true;

        object IList.this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
            }
        }

        int IList.Add(object value)
        {
            __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
            return -1;
        }

        void IList.Clear()
        {
            __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
        }

        private static bool IsCompatibleObject(object value)
        {
            return (value is T) || (value == null && default(T) == null);
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
                return IndexOf((T)value);
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
        }

        void IList.Remove(object value)
        {
            __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
        }

        void IList.RemoveAt(int index)
        {
            __ThrowHelper.ThrowNotSupportedException(__ResourceName.NotSupported_ReadOnlyCollection);
        }
    }
}