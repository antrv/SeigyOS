using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Byte: IComparable, IFormattable, IConvertible, IComparable<byte>, IEquatable<byte>
    {
        public const byte MinValue = 0;
        public const byte MaxValue = 0xFF;

        private readonly byte _value;

        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (!(value is byte))
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_MustBeByte));
            return _value - (byte)value;
        }

        public int CompareTo(byte value)
        {
            return _value - value;
        }

        public override bool Equals(object obj)
        {
            return obj is byte && _value == (byte)obj;
        }

        public bool Equals(byte obj)
        {
            return _value == obj;
        }

        public override int GetHashCode()
        {
            return _value;
        }

        [Pure]
        public static byte Parse(string s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [Pure]
        public static byte Parse(string s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.CurrentInfo);
        }

        [Pure]
        public static byte Parse(string s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        [Pure]
        public static byte Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.GetInstance(provider));
        }

        private static byte Parse(string s, NumberStyles style, NumberFormatInfo info)
        {
            int i;
            try
            {
                i = Number.ParseInt32(s, style, info);
            }
            catch (OverflowException e)
            {
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Byte), e);
            }

            if (i < MinValue || i > MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Byte));
            return (byte)i;
        }

        public static bool TryParse(string s, out byte result)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out byte result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out byte result)
        {
            result = 0;
            int i;
            if (!Number.TryParseInt32(s, style, info, out i))
                return false;
            if (i < MinValue || i > MaxValue)
                return false;
            result = (byte)i;
            return true;
        }

        [Pure]
        [SecuritySafeCritical]
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return Number.FormatInt32(_value, null, NumberFormatInfo.CurrentInfo);
        }

        [Pure]
        [SecuritySafeCritical]
        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return Number.FormatInt32(_value, format, NumberFormatInfo.CurrentInfo);
        }

        [Pure]
        [SecuritySafeCritical]
        public string ToString(IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return Number.FormatInt32(_value, null, NumberFormatInfo.GetInstance(provider));
        }

        [Pure]
        [SecuritySafeCritical]
        public string ToString(string format, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return Number.FormatInt32(_value, format, NumberFormatInfo.GetInstance(provider));
        }

        [Pure]
        public TypeCode GetTypeCode()
        {
            return TypeCode.Byte;
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
            return _value;
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
            return Convert.ToInt32(_value);
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
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Byte", "DateTime"));
        }

        object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType(this, type, provider);
        }
    }
}