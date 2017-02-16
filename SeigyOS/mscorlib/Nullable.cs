using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System
{
    [ComVisible(true)]
    public static class Nullable
    {
        [ComVisible(true)]
        public static int Compare<T>(T? n1, T? n2)
            where T: struct
        {
            if (n1.HasValue)
            {
                if (n2.HasValue)
                    return Comparer<T>.Default.Compare(n1._value, n2._value);
                return 1;
            }
            if (n2.HasValue)
                return -1;
            return 0;
        }

        [ComVisible(true)]
        public static bool Equals<T>(T? n1, T? n2) 
            where T: struct
        {
            if (n1.HasValue)
            {
                if (n2.HasValue)
                    return EqualityComparer<T>.Default.Equals(n1._value, n2._value);
                return false;
            }
            if (n2.HasValue)
                return false;
            return true;
        }

        public static Type GetUnderlyingType(Type nullableType)
        {
            if (nullableType == null)
                throw new ArgumentNullException(nameof(nullableType));
            Contract.EndContractBlock();

            Type result = null;
            if (nullableType.IsGenericType && !nullableType.IsGenericTypeDefinition)
            {
                Type genericType = nullableType.GetGenericTypeDefinition();
                if (ReferenceEquals(genericType, typeof(Nullable<>)))
                    result = nullableType.GetGenericArguments()[0];
            }
            return result;
        }
    }

    [Serializable]
    public struct Nullable<T>
        where T: struct
    {
        private readonly bool _hasValue;
        internal readonly T _value;

        public Nullable(T value)
        {
            _value = value;
            _hasValue = true;
        }

        public bool HasValue => _hasValue;

        public T Value
        {
            get
            {
                if (!_hasValue)
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_NoValue);
                return _value;
            }
        }

        public T GetValueOrDefault()
        {
            return _value;
        }

        public T GetValueOrDefault(T defaultValue)
        {
            return _hasValue ? _value : defaultValue;
        }

        public override bool Equals(object other)
        {
            if (!_hasValue)
                return other == null;
            if (other == null)
                return false;
            return _value.Equals(other);
        }

        public override int GetHashCode()
        {
            return _hasValue ? _value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return _hasValue ? _value.ToString() : "";
        }

        public static implicit operator T?(T value)
        {
            return value;
        }

        public static explicit operator T(T? value)
        {
            return value.Value;
        }
    }
}