using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace System
{
    [Serializable]
    [ComVisible(true)]
    public struct Boolean: IComparable, IConvertible, IComparable<bool>, IEquatable<bool>
    {
        public static readonly string TrueString = "True";
        public static readonly string FalseString = "False";

        private readonly bool _value;

        public override int GetHashCode()
        {
            return _value ? 1 : 0;
        }

        public override string ToString()
        {
            return _value ? "True" : "False";
        }

        public string ToString(IFormatProvider provider)
        {
            return _value ? "True" : "False";
        }

        public override bool Equals(object obj)
        {
            return obj is bool && _value == (bool)obj;
        }

        public bool Equals(bool obj)
        {
            return _value == obj;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (!(obj is bool))
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_MustBeBoolean));

            if (_value == (bool)obj)
                return 0;
            if (_value)
                return 1;
            return -1;
        }

        public int CompareTo(bool value)
        {
            if (_value == value)
                return 0;
            if (_value)
                return 1;
            return -1;
        }

        public static bool Parse(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            Contract.EndContractBlock();
            bool result;
            if (!TryParse(value, out result))
                throw new FormatException(__Resources.GetResourceString(__Resources.Format_BadBoolean));
            return result;
        }

        public static bool TryParse(string value, out bool result)
        {
            result = false;
            if (value == null)
                return false;

            if ("True".Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                result = true;
                return true;
            }

            if ("False".Equals(value, StringComparison.OrdinalIgnoreCase))
                return true;

            value = TrimWhiteSpaceAndNull(value);
            if ("True".Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                result = true;
                return true;
            }

            if ("False".Equals(value, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private static string TrimWhiteSpaceAndNull(string value)
        {
            int start = 0;
            int end = value.Length - 1;
            char nullChar = (char)0x0000;

            while (start < value.Length)
            {
                if (!char.IsWhiteSpace(value[start]) && value[start] != nullChar)
                    break;
                start++;
            }

            while (end >= start)
            {
                if (!char.IsWhiteSpace(value[end]) && value[end] != nullChar)
                    break;
                end--;
            }

            return value.Substring(start, end - start + 1);
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Boolean;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return _value;
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Boolean", "Char"));
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
            return Convert.ToDouble(_value);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(_value);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Boolean", "DateTime"));
        }

        object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType(this, type, provider);
        }
    }
}