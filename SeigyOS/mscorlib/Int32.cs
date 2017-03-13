using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.__Helpers;

namespace System
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct Int32: IComparable, IFormattable, IConvertible, IComparable<int>, IEquatable<int>
    {
        public const int MinValue = unchecked((int)0x80000000);
        public const int MaxValue = 0x7fffffff;

        private readonly int _value;

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
            throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_MustBeInt32));
        }

        public int CompareTo(int value)
        {
            if (_value < value)
                return -1;
            if (_value > value)
                return 1;
            return 0;
        }

        public override bool Equals(object obj)
        {
            return obj is int && _value == (int)obj;
        }

        public bool Equals(int obj)
        {
            return _value == obj;
        }

        public override int GetHashCode()
        {
            return _value;
        }

        [SecuritySafeCritical]
        [Pure]
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatInt32(_value, null, NumberFormatInfo.CurrentInfo);
        }

        [SecuritySafeCritical]
        [Pure]
        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatInt32(_value, format, NumberFormatInfo.CurrentInfo);
        }

        [SecuritySafeCritical]
        [Pure]
        public string ToString(IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatInt32(_value, null, NumberFormatInfo.GetInstance(provider));
        }

        [Pure]
        [SecuritySafeCritical]
        public string ToString(string format, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatInt32(_value, format, NumberFormatInfo.GetInstance(provider));
        }

        [Pure]
        public static int Parse(string s)
        {
            return NumberHelper.ParseInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [Pure]
        public static int Parse(string s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return NumberHelper.ParseInt32(s, style, NumberFormatInfo.CurrentInfo);
        }

        [Pure]
        public static int Parse(string s, IFormatProvider provider)
        {
            return NumberHelper.ParseInt32(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        [Pure]
        public static int Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return NumberHelper.ParseInt32(s, style, NumberFormatInfo.GetInstance(provider));
        }

        [Pure]
        public static bool TryParse(string s, out int result)
        {
            return NumberHelper.TryParseInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        [Pure]
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out int result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return NumberHelper.TryParseInt32(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        [Pure]
        public TypeCode GetTypeCode()
        {
            return TypeCode.Int32;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(_value);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(_value);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(_value);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(_value);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(_value);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(_value);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return _value;
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(_value);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(_value);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(_value);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(_value);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(_value);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(_value);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Int32", "DateTime"));
        }

        object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType(this, type, provider);
        }
    }
}