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
    public struct SByte: IComparable, IFormattable, IConvertible, IComparable<sbyte>, IEquatable<sbyte>
    {
        public const sbyte MinValue = unchecked((sbyte)0x80);
        public const sbyte MaxValue = 0x7F;

        private readonly sbyte _value;

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (!(obj is sbyte))
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_MustBeSByte));
            return _value - (sbyte)obj;
        }

        public int CompareTo(sbyte value)
        {
            return _value - value;
        }

        public override bool Equals(object obj)
        {
            return obj is sbyte && _value == (sbyte)obj;
        }

        public bool Equals(sbyte obj)
        {
            return _value == obj;
        }

        public override int GetHashCode()
        {
            return _value ^ _value << 8;
        }

        [SecuritySafeCritical]
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return Number.FormatInt32(_value, null, NumberFormatInfo.CurrentInfo);
        }

        [SecuritySafeCritical]
        public string ToString(IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return Number.FormatInt32(_value, null, NumberFormatInfo.GetInstance(provider));
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
                uint temp = (uint)(_value & 0x000000FF);
                return Number.FormatUInt32(temp, format, info);
            }
            return Number.FormatInt32(_value, format, info);
        }

        [CLSCompliant(false)]
        public static sbyte Parse(string s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static sbyte Parse(string s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static sbyte Parse(string s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        [CLSCompliant(false)]
        public static sbyte Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.GetInstance(provider));
        }

        private static sbyte Parse(string s, NumberStyles style, NumberFormatInfo info)
        {
            int i;
            try
            {
                i = Number.ParseInt32(s, style, info);
            }
            catch (OverflowException e)
            {
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte), e);
            }

            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            {
                if (i < 0 || i > byte.MaxValue)
                    throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte));
                return (sbyte)i;
            }

            if (i < MinValue || i > MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte));
            return (sbyte)i;
        }

        [CLSCompliant(false)]
        public static bool TryParse(string s, out sbyte result)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        [CLSCompliant(false)]
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out sbyte result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out sbyte result)
        {
            result = 0;
            int i;
            if (!Number.TryParseInt32(s, style, info, out i))
                return false;

            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            {
                if (i < 0 || i > byte.MaxValue)
                    return false;
                result = (sbyte)i;
                return true;
            }

            if (i < MinValue || i > MaxValue)
                return false;
            result = (sbyte)i;
            return true;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.SByte;
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
            return _value;
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
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "SByte", "DateTime"));
        }

        object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType(this, type, provider);
        }
    }
}