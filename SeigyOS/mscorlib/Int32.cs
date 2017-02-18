using System.Runtime.InteropServices;

namespace System
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct Int32: IComparable, IFormattable, IConvertible, IComparable<int>, IEquatable<int>
    {
        public const int MaxValue = 0x7fffffff;
        public const int MinValue = unchecked((int)0x80000000);

        private int _value;

        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (value is int)
            {
                int i = (int)value;
                if (_value < i)
                    return -1;
                if (_value > i)
                    return 1;
                return 0;
            }
            throw new ArgumentException(Environment.GetResourceString("Arg_MustBeInt32"));
        }

        public int CompareTo(int value)
        {
            if (_value < value) return -1;
            if (_value > value) return 1;
            return 0;
        }

        public override bool Equals(object obj)
        {
            return obj is int && this == (int)obj;
        }

        public bool Equals(int obj)
        {
            return this == obj;
        }

        public override int GetHashCode()
        {
            return this;
        }

        // TODO: members

        [Pure]
        public TypeCode GetTypeCode()
        {
            return TypeCode.Int32;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(m_value);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(m_value);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(m_value);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(m_value);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(m_value);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(m_value);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return m_value;
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(m_value);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(m_value);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(m_value);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(m_value);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(m_value);
        }

        Decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(m_value);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "Int32", "DateTime"));
        }

        object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType((IConvertible)this, type, provider);
        }
    }
}