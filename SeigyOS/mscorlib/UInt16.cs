using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;

namespace System
{
    [Serializable]
    [CLSCompliant(false)]
    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct UInt16: IComparable, IFormattable, IConvertible, IComparable<ushort>, IEquatable<ushort>
    {
        public const ushort MinValue = 0;
        public const ushort MaxValue = 0xFFFF;

        private readonly ushort _value;

        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (value is ushort)
                return _value - (ushort)value;
            throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_MustBeUInt16));
        }

        public int CompareTo(ushort value)
        {
            return _value - value;
        }

        public override bool Equals(object obj)
        {
            return obj is ushort && _value == (ushort)obj;
        }

        public bool Equals(ushort obj)
        {
            return _value == obj;
        }

        public override int GetHashCode()
        {
            return _value;
        }

        [SecuritySafeCritical]
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return Number.FormatUInt32(_value, null, NumberFormatInfo.CurrentInfo);
        }

        [SecuritySafeCritical]
        public string ToString(IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return Number.FormatUInt32(_value, null, NumberFormatInfo.GetInstance(provider));
        }

        [SecuritySafeCritical]
        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return Number.FormatUInt32(_value, format, NumberFormatInfo.CurrentInfo);
        }

        [SecuritySafeCritical]
        public string ToString(string format, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return Number.FormatUInt32(_value, format, NumberFormatInfo.GetInstance(provider));
        }

        [CLSCompliant(false)]
        public static ushort Parse(string s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static ushort Parse(string s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static ushort Parse(string s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        [CLSCompliant(false)]
        public static ushort Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.GetInstance(provider));
        }

        private static ushort Parse(string s, NumberStyles style, NumberFormatInfo info)
        {
            uint i;
            try
            {
                i = Number.ParseUInt32(s, style, info);
            }
            catch (OverflowException e)
            {
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt16), e);
            }

            if (i > MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt16));
            return (ushort)i;
        }

        [CLSCompliant(false)]
        public static bool TryParse(string s, out ushort result)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        [CLSCompliant(false)]
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ushort result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out ushort result)
        {
            result = 0;
            uint i;
            if (!Number.TryParseUInt32(s, style, info, out i))
                return false;
            if (i > MaxValue)
                return false;
            result = (ushort)i;
            return true;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.UInt16;
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
            return _value;
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
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "UInt16", "DateTime"));
        }

        object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType(this, type, provider);
        }
    }
}