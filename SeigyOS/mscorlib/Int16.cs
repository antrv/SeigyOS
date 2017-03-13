using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.__Helpers;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Int16: IComparable, IFormattable, IConvertible, IComparable<short>, IEquatable<short>
    {
        public const short MinValue = unchecked((short)0x8000);
        public const short MaxValue = 0x7FFF;

        private readonly short _value;

        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (value is short)
                return _value - (short)value;
            throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_MustBeInt16));
        }

        public int CompareTo(short value)
        {
            return _value - value;
        }

        public override bool Equals(object obj)
        {
            return obj is short && _value == (short)obj;
        }

        public bool Equals(short obj)
        {
            return _value == obj;
        }

        public override int GetHashCode()
        {
            return (ushort)_value | (_value << 16);
        }

        [SecuritySafeCritical]
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatInt32(_value, null, NumberFormatInfo.CurrentInfo);
        }

        [SecuritySafeCritical]
        public string ToString(IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatInt32(_value, null, NumberFormatInfo.GetInstance(provider));
        }

        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return ToString(format, NumberFormatInfo.CurrentInfo);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return ToString(format, NumberFormatInfo.GetInstance(provider));
        }

        [SecuritySafeCritical]
        private string ToString(string format, NumberFormatInfo info)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            if (_value < 0 && format != null && format.Length > 0 && (format[0] == 'X' || format[0] == 'x'))
            {
                uint temp = (uint)(_value & 0x0000FFFF);
                return NumberHelper.FormatUInt32(temp, format, info);
            }
            return NumberHelper.FormatInt32(_value, format, info);
        }

        public static short Parse(string s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static short Parse(string s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.CurrentInfo);
        }

        public static short Parse(string s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        public static short Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.GetInstance(provider));
        }

        private static short Parse(string s, NumberStyles style, NumberFormatInfo info)
        {
            int i;
            try
            {
                i = NumberHelper.ParseInt32(s, style, info);
            }
            catch (OverflowException e)
            {
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int16), e);
            }
            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            {
                if (i < 0 || i > ushort.MaxValue)
                    throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int16));
                return (short)i;
            }

            if (i < MinValue || i > MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int16));
            return (short)i;
        }

        public static bool TryParse(string s, out short result)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out short result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out short result)
        {
            result = 0;
            int i;
            if (!NumberHelper.TryParseInt32(s, style, info, out i))
                return false;

            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            {
                if (i < 0 || i > ushort.MaxValue)
                    return false;
                result = (short)i;
                return true;
            }

            if (i < MinValue || i > MaxValue)
                return false;
            result = (short)i;
            return true;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Int16;
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
            return _value;
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
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Int16", "DateTime"));
        }

        object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType(this, type, provider);
        }
    }
}