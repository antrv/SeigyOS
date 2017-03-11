using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public sealed class CharEnumerator: ICloneable, IEnumerator<char>
    {
        private string _str;
        private int _index;
        private char _currentElement;

        internal CharEnumerator(string str)
        {
            Contract.Requires(str != null);
            _str = str;
            _index = -1;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool MoveNext()
        {
            if (_index < _str.Length - 1)
            {
                _index++;
                _currentElement = _str[_index];
                return true;
            }
            _index = _str.Length;
            return false;
        }

        public void Dispose()
        {
            if (_str != null)
                _index = _str.Length;
            _str = null;
        }

        object IEnumerator.Current
        {
            get
            {
                if (_index == -1)
                    throw new InvalidOperationException(__Resources.GetResourceString(__Resources.InvalidOperation_EnumNotStarted));
                if (_index >= _str.Length)
                    throw new InvalidOperationException(__Resources.GetResourceString(__Resources.InvalidOperation_EnumEnded));
                return _currentElement;
            }
        }

        public char Current
        {
            get
            {
                if (_index == -1)
                    throw new InvalidOperationException(__Resources.GetResourceString(__Resources.InvalidOperation_EnumNotStarted));
                if (_index >= _str.Length)
                    throw new InvalidOperationException(__Resources.GetResourceString(__Resources.InvalidOperation_EnumEnded));
                return _currentElement;
            }
        }

        public void Reset()
        {
            _currentElement = (char)0;
            _index = -1;
        }
    }
}