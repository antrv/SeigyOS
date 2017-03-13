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
    public struct Single: IComparable, IFormattable, IConvertible, IComparable<float>, IEquatable<float>
    {
        public const float MinValue = (float)-3.40282346638528859e+38;
        public const float MaxValue = (float)3.40282346638528859e+38;

        public const float Epsilon = (float)1.4e-45;
        public const float PositiveInfinity = (float)1.0 / (float)0.0;
        public const float NegativeInfinity = (float)-1.0 / (float)0.0;
        public const float NaN = (float)0.0 / (float)0.0;

        private readonly float _value;

        [Pure]
        [SecuritySafeCritical]
        public static unsafe bool IsInfinity(float f)
        {
            return (*(int*)(&f) & 0x7FFFFFFF) == 0x7F800000;
        }

        [Pure]
        [SecuritySafeCritical]
        public static unsafe bool IsPositiveInfinity(float f)
        {
            return *(int*)(&f) == 0x7F800000;
        }

        [Pure]
        [SecuritySafeCritical]
        public static unsafe bool IsNegativeInfinity(float f)
        {
            return *(int*)(&f) == unchecked((int)0xFF800000);
        }

        [Pure]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public static unsafe bool IsNaN(float f)
        {
            return (*(int*)(&f) & 0x7FFFFFFF) > 0x7F800000;
        }

        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }
            if (value is float)
            {
                float f = (float)value;
                if (_value < f)
                    return -1;
                if (_value > f)
                    return 1;
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_value == f)
                    return 0;
                if (IsNaN(_value))
                    return IsNaN(f) ? 0 : -1;
                return 1;
            }
            throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_MustBeSingle));
        }

        public int CompareTo(float value)
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
            if (!(obj is float))
                return false;
            float temp = (float)obj;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (temp == _value)
                return true;
            return IsNaN(temp) && IsNaN(_value);
        }

        public bool Equals(float obj)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (obj == _value)
                return true;
            return IsNaN(obj) && IsNaN(_value);
        }

        [SecuritySafeCritical]
        public override unsafe int GetHashCode()
        {
            float f = _value;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (f == 0.0f)
                return 0;
            int v = *(int*)(&f);
            return v;
        }

        [SecuritySafeCritical]
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatSingle(_value, null, NumberFormatInfo.CurrentInfo);
        }

        [SecuritySafeCritical]
        public string ToString(IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatSingle(_value, null, NumberFormatInfo.GetInstance(provider));
        }

        [SecuritySafeCritical]
        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatSingle(_value, format, NumberFormatInfo.CurrentInfo);
        }

        [SecuritySafeCritical]
        public string ToString(string format, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatSingle(_value, format, NumberFormatInfo.GetInstance(provider));
        }

        public static float Parse(string s)
        {
            return NumberHelper.ParseSingle(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo);
        }

        public static float Parse(string s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return NumberHelper.ParseSingle(s, style, NumberFormatInfo.CurrentInfo);
        }

        public static float Parse(string s, IFormatProvider provider)
        {
            return NumberHelper.ParseSingle(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.GetInstance(provider));
        }

        public static float Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return NumberHelper.ParseSingle(s, style, NumberFormatInfo.GetInstance(provider));
        }

        public static bool TryParse(string s, out float result)
        {
            return TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out float result)
        {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out float result)
        {
            if (s == null)
            {
                result = 0;
                return false;
            }
            bool success = NumberHelper.TryParseSingle(s, style, info, out result);
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
            return TypeCode.Single;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(_value);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Single", "Char"));
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
            return _value;
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
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Single", "DateTime"));
        }

        object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType(this, type, provider);
        }
    }
}