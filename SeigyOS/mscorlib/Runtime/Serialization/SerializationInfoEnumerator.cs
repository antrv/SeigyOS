using System.Collections;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
    [ComVisible(true)]
    public sealed class SerializationInfoEnumerator: IEnumerator
    {
        private readonly string[] _members;
        private readonly object[] _data;
        private readonly Type[] _types;
        private readonly int _numItems;
        private int _currItem;
        private bool _current;

        internal SerializationInfoEnumerator(string[] members, object[] info, Type[] types, int numItems)
        {
            Contract.Assert(members != null, "[SerializationInfoEnumerator.ctor]members!=null");
            Contract.Assert(info != null, "[SerializationInfoEnumerator.ctor]info!=null");
            Contract.Assert(types != null, "[SerializationInfoEnumerator.ctor]types!=null");
            Contract.Assert(numItems >= 0, "[SerializationInfoEnumerator.ctor]numItems>=0");
            Contract.Assert(members.Length >= numItems, "[SerializationInfoEnumerator.ctor]members.Length>=numItems");
            Contract.Assert(info.Length >= numItems, "[SerializationInfoEnumerator.ctor]info.Length>=numItems");
            Contract.Assert(types.Length >= numItems, "[SerializationInfoEnumerator.ctor]types.Length>=numItems");

            _members = members;
            _data = info;
            _types = types;
            _numItems = numItems - 1;
            _currItem = -1;
            _current = false;
        }

        public bool MoveNext()
        {
            if (_currItem < _numItems)
            {
                _currItem++;
                _current = true;
            }
            else
            {
                _current = false;
            }
            return _current;
        }

        /// <internalonly/>
        object IEnumerator.Current
        {
            get
            {
                if (_current == false)
                    throw new InvalidOperationException(__Resources.GetResourceString("InvalidOperation_EnumOpCantHappen"));
                return new SerializationEntry(_members[_currItem], _data[_currItem], _types[_currItem]);
            }
        }

        public SerializationEntry Current
        {
            get
            {
                if (_current == false)
                    throw new InvalidOperationException(__Resources.GetResourceString("InvalidOperation_EnumOpCantHappen"));
                return new SerializationEntry(_members[_currItem], _data[_currItem], _types[_currItem]);
            }
        }

        public void Reset()
        {
            _currItem = -1;
            _current = false;
        }

        public string Name
        {
            get
            {
                if (_current == false)
                    throw new InvalidOperationException(__Resources.GetResourceString("InvalidOperation_EnumOpCantHappen"));
                return _members[_currItem];
            }
        }

        public object Value
        {
            get
            {
                if (_current == false)
                    throw new InvalidOperationException(__Resources.GetResourceString("InvalidOperation_EnumOpCantHappen"));
                return _data[_currItem];
            }
        }

        public Type ObjectType
        {
            get
            {
                if (_current == false)
                    throw new InvalidOperationException(__Resources.GetResourceString("InvalidOperation_EnumOpCantHappen"));
                return _types[_currItem];
            }
        }
    }
}