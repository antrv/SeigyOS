using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.__Helpers;

namespace System
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct Double: IComparable, IFormattable, IConvertible, IComparable<double>, IEquatable<double>
    {
        public const double MinValue = -1.7976931348623157E+308;
        public const double MaxValue = 1.7976931348623157E+308;
        public const double Epsilon = 4.9406564584124654E-324;
        public const double NegativeInfinity = -1.0 / 0.0;
        public const double PositiveInfinity = 1.0 / 0.0;
        public const double NaN = 0.0 / 0.0;

        private readonly double _value;

        internal static double NegativeZero = BitConverter.Int64BitsToDouble(unchecked((long)0x8000000000000000));

        [Pure]
        [SecuritySafeCritical]
        public static unsafe bool IsInfinity(double d)
        {
            return (*(long*)(&d) & 0x7FFFFFFFFFFFFFFF) == 0x7FF0000000000000;
        }

        [Pure]
        public static bool IsPositiveInfinity(double d)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return d == PositiveInfinity;
        }

        [Pure]
        public static bool IsNegativeInfinity(double d)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return d == NegativeInfinity;
        }

        [Pure]
        [SecuritySafeCritical]
        internal static unsafe bool IsNegative(double d)
        {
            return (*(ulong*)(&d) & 0x8000000000000000) == 0x8000000000000000;
        }

        [Pure]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public static unsafe bool IsNaN(double d)
        {
            return (*(ulong*)(&d) & 0x7FFFFFFFFFFFFFFFL) > 0x7FF0000000000000L;
        }

        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (value is double)
            {
                double d = (double)value;
                if (_value < d)
                    return -1;
                if (_value > d)
                    return 1;
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_value == d)
                    return 0;
                if (IsNaN(_value))
                    return IsNaN(d) ? 0 : -1;
                return 1;
            }
            throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_MustBeDouble));
        }

        public int CompareTo(double value)
        {
            if (_value < value)
                return -1;
            if (_value > value)
                return 1;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_value == value)
                return 0;
            if (IsNaN(_value))
                return IsNaN(value) ? 0 : -1;
            return 1;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is double))
                return false;
            double temp = ((double)obj)._value;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (temp == _value)
                return true;
            return IsNaN(temp) && IsNaN(_value);
        }

        public bool Equals(double obj)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (obj == _value)
                return true;
            return IsNaN(obj) && IsNaN(_value);
        }

        [SecuritySafeCritical]
        public override unsafe int GetHashCode()
        {
            double d = _value;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (d == 0.0)
                return 0;
            long value = *(long*)(&d);
            return unchecked((int)value) ^ ((int)(value >> 32));
        }

        [SecuritySafeCritical]
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatDouble(_value, null, NumberFormatInfo.CurrentInfo);
        }

        [SecuritySafeCritical]
        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatDouble(_value, format, NumberFormatInfo.CurrentInfo);
        }

        [SecuritySafeCritical]
        public string ToString(IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatDouble(_value, null, NumberFormatInfo.GetInstance(provider));
        }

        [SecuritySafeCritical]
        public string ToString(string format, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatDouble(_value, format, NumberFormatInfo.GetInstance(provider));
        }

        public static double Parse(string s)
        {
            return NumberHelper.ParseDouble(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo);
        }

        public static double Parse(string s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return NumberHelper.ParseDouble(s, style, NumberFormatInfo.CurrentInfo);
        }

        public static double Parse(string s, IFormatProvider provider)
        {
            return NumberHelper.ParseDouble(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.GetInstance(provider));
        }

        public static double Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return NumberHelper.ParseDouble(s, style, NumberFormatInfo.GetInstance(provider));
        }

        public static bool TryParse(string s, out double result)
        {
            return TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out double result)
        {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out double result)
        {
            if (s == null)
            {
                result = 0;
                return false;
            }
            bool success = NumberHelper.TryParseDouble(s, style, info, out result);
            if (!success)
            {
                string sTrim = s.Trim();
                if (sTrim.Equals(info.PositiveInfinitySymbol))
                    result = PositiveInfinity;
                else if (sTrim.Equals(info.NegativeInfinitySymbol))
                    result = NegativeInfinity;
                else if (sTrim.Equals(info.NaNSymbol))
                    result = NaN;
                else
                    return false;
            }
            return true;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Double;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(_value);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Double", "Char"));
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
            return _value;
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(_value);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Double", "DateTime"));
        }

        object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType(this, type, provider);
        }
    }
}