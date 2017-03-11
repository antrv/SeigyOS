using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System
{
    public static class Convert
    {
        private static readonly Type[] _convertTypes =
        {
            typeof(Empty),
            typeof(object),
            typeof(DBNull),
            typeof(bool),
            typeof(char),
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(DateTime),
            typeof(object),
            typeof(string)
        };

        private static readonly Type _enumType = typeof(Enum);

        private static readonly char[] _base64Table =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
            'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
            'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
            't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', '+', '/', '='
        };

        private const int cBase64LineBreakPosition = 76;

        // ReSharper disable once InconsistentNaming
        public static readonly object DBNull = System.DBNull.Value;

        [Pure]
        public static TypeCode GetTypeCode(object value)
        {
            if (value == null)
                return TypeCode.Empty;
            IConvertible temp = value as IConvertible;
            if (temp != null)
                return temp.GetTypeCode();
            return TypeCode.Object;
        }

        [Pure]
        // ReSharper disable once InconsistentNaming
        public static bool IsDBNull(object value)
        {
            if (value == System.DBNull.Value)
                return true;
            IConvertible convertible = value as IConvertible;
            return convertible != null && convertible.GetTypeCode() == TypeCode.DBNull;
        }

        public static object ChangeType(object value, TypeCode typeCode)
        {
            return ChangeType(value, typeCode, Thread.CurrentThread.CurrentCulture);
        }

        public static object ChangeType(object value, TypeCode typeCode, IFormatProvider provider)
        {
            if (value == null && (typeCode == TypeCode.Empty || typeCode == TypeCode.String || typeCode == TypeCode.Object))
                return null;

            IConvertible v = value as IConvertible;
            if (v == null)
                throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_IConvertible));

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return v.ToBoolean(provider);
                case TypeCode.Char:
                    return v.ToChar(provider);
                case TypeCode.SByte:
                    return v.ToSByte(provider);
                case TypeCode.Byte:
                    return v.ToByte(provider);
                case TypeCode.Int16:
                    return v.ToInt16(provider);
                case TypeCode.UInt16:
                    return v.ToUInt16(provider);
                case TypeCode.Int32:
                    return v.ToInt32(provider);
                case TypeCode.UInt32:
                    return v.ToUInt32(provider);
                case TypeCode.Int64:
                    return v.ToInt64(provider);
                case TypeCode.UInt64:
                    return v.ToUInt64(provider);
                case TypeCode.Single:
                    return v.ToSingle(provider);
                case TypeCode.Double:
                    return v.ToDouble(provider);
                case TypeCode.Decimal:
                    return v.ToDecimal(provider);
                case TypeCode.DateTime:
                    return v.ToDateTime(provider);
                case TypeCode.String:
                    return v.ToString(provider);
                case TypeCode.Object:
                    return value;
                case TypeCode.DBNull:
                    throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_DBNull));
                case TypeCode.Empty:
                    throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_Empty));
                default:
                    throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_UnknownTypeCode));
            }
        }

        internal static object DefaultToType(IConvertible value, Type targetType, IFormatProvider provider)
        {
            Contract.Requires(value != null, "[Convert.DefaultToType]value!=null");
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            Contract.EndContractBlock();

            if (value.GetType() == targetType)
                return value;
            if (targetType == _convertTypes[(int)TypeCode.Boolean])
                return value.ToBoolean(provider);
            if (targetType == _convertTypes[(int)TypeCode.Char])
                return value.ToChar(provider);
            if (targetType == _convertTypes[(int)TypeCode.SByte])
                return value.ToSByte(provider);
            if (targetType == _convertTypes[(int)TypeCode.Byte])
                return value.ToByte(provider);
            if (targetType == _convertTypes[(int)TypeCode.Int16])
                return value.ToInt16(provider);
            if (targetType == _convertTypes[(int)TypeCode.UInt16])
                return value.ToUInt16(provider);
            if (targetType == _convertTypes[(int)TypeCode.Int32])
                return value.ToInt32(provider);
            if (targetType == _convertTypes[(int)TypeCode.UInt32])
                return value.ToUInt32(provider);
            if (targetType == _convertTypes[(int)TypeCode.Int64])
                return value.ToInt64(provider);
            if (targetType == _convertTypes[(int)TypeCode.UInt64])
                return value.ToUInt64(provider);
            if (targetType == _convertTypes[(int)TypeCode.Single])
                return value.ToSingle(provider);
            if (targetType == _convertTypes[(int)TypeCode.Double])
                return value.ToDouble(provider);
            if (targetType == _convertTypes[(int)TypeCode.Decimal])
                return value.ToDecimal(provider);
            if (targetType == _convertTypes[(int)TypeCode.DateTime])
                return value.ToDateTime(provider);
            if (targetType == _convertTypes[(int)TypeCode.String])
                return value.ToString(provider);
            if (targetType == _convertTypes[(int)TypeCode.Object])
                return value;
            if (targetType == _enumType)
                return (Enum)value;
            if (targetType == _convertTypes[(int)TypeCode.DBNull])
                throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_DBNull));
            if (targetType == _convertTypes[(int)TypeCode.Empty])
                throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_Empty));

            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, value.GetType().FullName, targetType.FullName));
        }

        public static object ChangeType(object value, Type conversionType)
        {
            return ChangeType(value, conversionType, Thread.CurrentThread.CurrentCulture);
        }

        public static object ChangeType(object value, Type conversionType, IFormatProvider provider)
        {
            if (conversionType == null)
                throw new ArgumentNullException(nameof(conversionType));
            Contract.EndContractBlock();

            if (value == null)
            {
                if (conversionType.IsValueType)
                    throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_CannotCastNullToValueType));
                return null;
            }

            IConvertible ic = value as IConvertible;
            if (ic == null)
            {
                if (value.GetType() == conversionType)
                    return value;
                throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_IConvertible));
            }

            if (conversionType == _convertTypes[(int)TypeCode.Boolean])
                return ic.ToBoolean(provider);
            if (conversionType == _convertTypes[(int)TypeCode.Char])
                return ic.ToChar(provider);
            if (conversionType == _convertTypes[(int)TypeCode.SByte])
                return ic.ToSByte(provider);
            if (conversionType == _convertTypes[(int)TypeCode.Byte])
                return ic.ToByte(provider);
            if (conversionType == _convertTypes[(int)TypeCode.Int16])
                return ic.ToInt16(provider);
            if (conversionType == _convertTypes[(int)TypeCode.UInt16])
                return ic.ToUInt16(provider);
            if (conversionType == _convertTypes[(int)TypeCode.Int32])
                return ic.ToInt32(provider);
            if (conversionType == _convertTypes[(int)TypeCode.UInt32])
                return ic.ToUInt32(provider);
            if (conversionType == _convertTypes[(int)TypeCode.Int64])
                return ic.ToInt64(provider);
            if (conversionType == _convertTypes[(int)TypeCode.UInt64])
                return ic.ToUInt64(provider);
            if (conversionType == _convertTypes[(int)TypeCode.Single])
                return ic.ToSingle(provider);
            if (conversionType == _convertTypes[(int)TypeCode.Double])
                return ic.ToDouble(provider);
            if (conversionType == _convertTypes[(int)TypeCode.Decimal])
                return ic.ToDecimal(provider);
            if (conversionType == _convertTypes[(int)TypeCode.DateTime])
                return ic.ToDateTime(provider);
            if (conversionType == _convertTypes[(int)TypeCode.String])
                return ic.ToString(provider);
            if (conversionType == _convertTypes[(int)TypeCode.Object])
                return value;

            return ic.ToType(conversionType, provider);
        }

        public static bool ToBoolean(object value)
        {
            return value != null && ((IConvertible)value).ToBoolean(null);
        }

        public static bool ToBoolean(object value, IFormatProvider provider)
        {
            return value != null && ((IConvertible)value).ToBoolean(provider);
        }

        public static bool ToBoolean(bool value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(sbyte value)
        {
            return value != 0;
        }

        public static bool ToBoolean(char value)
        {
            return ((IConvertible)value).ToBoolean(null);
        }

        public static bool ToBoolean(byte value)
        {
            return value != 0;
        }

        public static bool ToBoolean(short value)
        {
            return value != 0;
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(ushort value)
        {
            return value != 0;
        }

        public static bool ToBoolean(int value)
        {
            return value != 0;
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(uint value)
        {
            return value != 0;
        }

        public static bool ToBoolean(long value)
        {
            return value != 0;
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(ulong value)
        {
            return value != 0;
        }

        public static bool ToBoolean(string value)
        {
            if (value == null)
                return false;
            return bool.Parse(value);
        }

        public static bool ToBoolean(string value, IFormatProvider provider)
        {
            return value != null && bool.Parse(value);
        }

        public static bool ToBoolean(float value)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return value != 0;
        }

        public static bool ToBoolean(double value)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return value != 0;
        }

        public static bool ToBoolean(decimal value)
        {
            return value != 0;
        }

        public static bool ToBoolean(DateTime value)
        {
            return ((IConvertible)value).ToBoolean(null);
        }

        public static char ToChar(object value)
        {
            return value == null ? (char)0 : ((IConvertible)value).ToChar(null);
        }

        public static char ToChar(object value, IFormatProvider provider)
        {
            return value == null ? (char)0 : ((IConvertible)value).ToChar(provider);
        }

        public static char ToChar(bool value)
        {
            return ((IConvertible)value).ToChar(null);
        }

        public static char ToChar(char value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static char ToChar(sbyte value)
        {
            if (value < 0)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Char));
            Contract.EndContractBlock();
            return (char)value;
        }

        public static char ToChar(byte value)
        {
            return (char)value;
        }

        public static char ToChar(short value)
        {
            if (value < 0)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Char));
            Contract.EndContractBlock();
            return (char)value;
        }

        [CLSCompliant(false)]
        public static char ToChar(ushort value)
        {
            return (char)value;
        }

        public static char ToChar(int value)
        {
            if (value < 0 || value > char.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Char));
            Contract.EndContractBlock();
            return (char)value;
        }

        [CLSCompliant(false)]
        public static char ToChar(uint value)
        {
            if (value > char.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Char));
            Contract.EndContractBlock();
            return (char)value;
        }

        public static char ToChar(long value)
        {
            if (value < 0 || value > char.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Char));
            Contract.EndContractBlock();
            return (char)value;
        }

        [CLSCompliant(false)]
        public static char ToChar(ulong value)
        {
            if (value > char.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Char));
            Contract.EndContractBlock();
            return (char)value;
        }

        public static char ToChar(string value)
        {
            return ToChar(value, null);
        }

        public static char ToChar(string value, IFormatProvider provider)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            Contract.EndContractBlock();
            if (value.Length != 1)
                throw new FormatException(__Resources.GetResourceString(__Resources.Format_NeedSingleChar));
            return value[0];
        }

        public static char ToChar(float value)
        {
            return ((IConvertible)value).ToChar(null);
        }

        public static char ToChar(double value)
        {
            return ((IConvertible)value).ToChar(null);
        }

        public static char ToChar(decimal value)
        {
            return ((IConvertible)value).ToChar(null);
        }

        public static char ToChar(DateTime value)
        {
            return ((IConvertible)value).ToChar(null);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(object value)
        {
            return value == null ? (sbyte)0 : ((IConvertible)value).ToSByte(null);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(object value, IFormatProvider provider)
        {
            return value == null ? (sbyte)0 : ((IConvertible)value).ToSByte(provider);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(bool value)
        {
            return value ? (sbyte)1 : (sbyte)0;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(sbyte value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(char value)
        {
            if (value > sbyte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte));
            Contract.EndContractBlock();
            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(byte value)
        {
            if (value > sbyte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte));
            Contract.EndContractBlock();
            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(short value)
        {
            if (value < sbyte.MinValue || value > sbyte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte));
            Contract.EndContractBlock();
            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(ushort value)
        {
            if (value > sbyte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte));
            Contract.EndContractBlock();
            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(int value)
        {
            if (value < sbyte.MinValue || value > sbyte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte));
            Contract.EndContractBlock();
            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(uint value)
        {
            if (value > sbyte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte));
            Contract.EndContractBlock();
            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(long value)
        {
            if (value < sbyte.MinValue || value > sbyte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte));
            Contract.EndContractBlock();
            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(ulong value)
        {
            if (value > (ulong)sbyte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte));
            Contract.EndContractBlock();
            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(float value)
        {
            return ToSByte((double)value);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(double value)
        {
            return ToSByte(ToInt32(value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(decimal value)
        {
            return decimal.ToSByte(decimal.Round(value, 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(string value)
        {
            if (value == null)
                return 0;
            return sbyte.Parse(value, CultureInfo.CurrentCulture);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(string value, IFormatProvider provider)
        {
            return sbyte.Parse(value, NumberStyles.Integer, provider);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(DateTime value)
        {
            return ((IConvertible)value).ToSByte(null);
        }

        public static byte ToByte(object value)
        {
            return value == null ? (byte)0 : ((IConvertible)value).ToByte(null);
        }

        public static byte ToByte(object value, IFormatProvider provider)
        {
            return value == null ? (byte)0 : ((IConvertible)value).ToByte(provider);
        }

        public static byte ToByte(bool value)
        {
            return value ? (byte)1 : (byte)0;
        }

        public static byte ToByte(byte value)
        {
            return value;
        }

        public static byte ToByte(char value)
        {
            if (value > byte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Byte));
            Contract.EndContractBlock();
            return (byte)value;
        }

        [CLSCompliant(false)]
        public static byte ToByte(sbyte value)
        {
            if (value < byte.MinValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Byte));
            Contract.EndContractBlock();
            return (byte)value;
        }

        public static byte ToByte(short value)
        {
            if (value < byte.MinValue || value > byte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Byte));
            Contract.EndContractBlock();
            return (byte)value;
        }

        [CLSCompliant(false)]
        public static byte ToByte(ushort value)
        {
            if (value > byte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Byte));
            Contract.EndContractBlock();
            return (byte)value;
        }

        public static byte ToByte(int value)
        {
            if (value < byte.MinValue || value > byte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Byte));
            Contract.EndContractBlock();
            return (byte)value;
        }

        [CLSCompliant(false)]
        public static byte ToByte(uint value)
        {
            if (value > byte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Byte));
            Contract.EndContractBlock();
            return (byte)value;
        }

        public static byte ToByte(long value)
        {
            if (value < byte.MinValue || value > byte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Byte));
            Contract.EndContractBlock();
            return (byte)value;
        }

        [CLSCompliant(false)]
        public static byte ToByte(ulong value)
        {
            if (value > byte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Byte));
            Contract.EndContractBlock();
            return (byte)value;
        }

        public static byte ToByte(float value)
        {
            return ToByte((double)value);
        }

        public static byte ToByte(double value)
        {
            return ToByte(ToInt32(value));
        }

        public static byte ToByte(decimal value)
        {
            return decimal.ToByte(decimal.Round(value, 0));
        }

        public static byte ToByte(string value)
        {
            if (value == null)
                return 0;
            return byte.Parse(value, CultureInfo.CurrentCulture);
        }

        public static byte ToByte(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return byte.Parse(value, NumberStyles.Integer, provider);
        }

        public static byte ToByte(DateTime value)
        {
            return ((IConvertible)value).ToByte(null);
        }

        public static short ToInt16(object value)
        {
            return value == null ? (short)0 : ((IConvertible)value).ToInt16(null);
        }

        public static short ToInt16(object value, IFormatProvider provider)
        {
            return value == null ? (short)0 : ((IConvertible)value).ToInt16(provider);
        }

        public static short ToInt16(bool value)
        {
            return value ? (short)1 : (short)0;
        }

        public static short ToInt16(char value)
        {
            if (value > short.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int16));
            Contract.EndContractBlock();
            return (short)value;
        }

        [CLSCompliant(false)]
        public static short ToInt16(sbyte value)
        {
            return value;
        }

        public static short ToInt16(byte value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static short ToInt16(ushort value)
        {
            if (value > short.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int16));
            Contract.EndContractBlock();
            return (short)value;
        }

        public static short ToInt16(int value)
        {
            if (value < short.MinValue || value > short.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int16));
            Contract.EndContractBlock();
            return (short)value;
        }

        [CLSCompliant(false)]
        public static short ToInt16(uint value)
        {
            if (value > short.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int16));
            Contract.EndContractBlock();
            return (short)value;
        }

        public static short ToInt16(short value)
        {
            return value;
        }

        public static short ToInt16(long value)
        {
            if (value < short.MinValue || value > short.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int16));
            Contract.EndContractBlock();
            return (short)value;
        }

        [CLSCompliant(false)]
        public static short ToInt16(ulong value)
        {
            if (value > (ulong)short.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int16));
            Contract.EndContractBlock();
            return (short)value;
        }

        public static short ToInt16(float value)
        {
            return ToInt16((double)value);
        }

        public static short ToInt16(double value)
        {
            return ToInt16(ToInt32(value));
        }

        public static short ToInt16(decimal value)
        {
            return decimal.ToInt16(decimal.Round(value, 0));
        }

        public static short ToInt16(string value)
        {
            if (value == null)
                return 0;
            return short.Parse(value, CultureInfo.CurrentCulture);
        }

        public static short ToInt16(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return short.Parse(value, NumberStyles.Integer, provider);
        }

        public static short ToInt16(DateTime value)
        {
            return ((IConvertible)value).ToInt16(null);
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(object value)
        {
            return value == null ? (ushort)0 : ((IConvertible)value).ToUInt16(null);
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(object value, IFormatProvider provider)
        {
            return value == null ? (ushort)0 : ((IConvertible)value).ToUInt16(provider);
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(bool value)
        {
            return value ? (ushort)1 : (ushort)0;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(char value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(sbyte value)
        {
            if (value < 0)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt16));
            Contract.EndContractBlock();
            return (ushort)value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(byte value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(short value)
        {
            if (value < 0)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt16));
            Contract.EndContractBlock();
            return (ushort)value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(int value)
        {
            if (value < 0 || value > ushort.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt16));
            Contract.EndContractBlock();
            return (ushort)value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(ushort value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(uint value)
        {
            if (value > ushort.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt16));
            Contract.EndContractBlock();
            return (ushort)value;
        }


        [CLSCompliant(false)]
        public static ushort ToUInt16(long value)
        {
            if (value < 0 || value > ushort.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt16));
            Contract.EndContractBlock();
            return (ushort)value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(ulong value)
        {
            if (value > ushort.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt16));
            Contract.EndContractBlock();
            return (ushort)value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(float value)
        {
            return ToUInt16((double)value);
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(double value)
        {
            return ToUInt16(ToInt32(value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(decimal value)
        {
            return decimal.ToUInt16(decimal.Round(value, 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(string value)
        {
            if (value == null)
                return 0;
            return ushort.Parse(value, CultureInfo.CurrentCulture);
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return ushort.Parse(value, NumberStyles.Integer, provider);
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(DateTime value)
        {
            return ((IConvertible)value).ToUInt16(null);
        }

        public static int ToInt32(object value)
        {
            return value == null ? 0 : ((IConvertible)value).ToInt32(null);
        }

        public static int ToInt32(object value, IFormatProvider provider)
        {
            return value == null ? 0 : ((IConvertible)value).ToInt32(provider);
        }

        public static int ToInt32(bool value)
        {
            return value ? 1 : 0;
        }

        public static int ToInt32(char value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static int ToInt32(sbyte value)
        {
            return value;
        }

        public static int ToInt32(byte value)
        {
            return value;
        }

        public static int ToInt32(short value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static int ToInt32(ushort value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static int ToInt32(uint value)
        {
            if (value > int.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int32));
            Contract.EndContractBlock();
            return (int)value;
        }

        public static int ToInt32(int value)
        {
            return value;
        }

        public static int ToInt32(long value)
        {
            if (value < int.MinValue || value > int.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int32));
            Contract.EndContractBlock();
            return (int)value;
        }

        [CLSCompliant(false)]
        public static int ToInt32(ulong value)
        {
            if (value > int.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int32));
            Contract.EndContractBlock();
            return (int)value;
        }

        public static int ToInt32(float value)
        {
            return ToInt32((double)value);
        }

        public static int ToInt32(double value)
        {
            if (value >= 0)
            {
                if (value < 2147483647.5)
                {
                    int result = (int)value;
                    double dif = value - result;
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (dif > 0.5 || dif == 0.5 && (result & 1) != 0)
                        result++;
                    return result;
                }
            }
            else
            {
                if (value >= -2147483648.5)
                {
                    int result = (int)value;
                    double dif = value - result;
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (dif < -0.5 || dif == -0.5 && (result & 1) != 0)
                        result--;
                    return result;
                }
            }
            throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int32));
        }

        [SecuritySafeCritical]
        public static int ToInt32(decimal value)
        {
            return decimal.FCallToInt32(value);
        }

        public static int ToInt32(string value)
        {
            if (value == null)
                return 0;
            return int.Parse(value, CultureInfo.CurrentCulture);
        }

        public static int ToInt32(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return int.Parse(value, NumberStyles.Integer, provider);
        }

        public static int ToInt32(DateTime value)
        {
            return ((IConvertible)value).ToInt32(null);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(object value)
        {
            return value == null ? 0 : ((IConvertible)value).ToUInt32(null);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(object value, IFormatProvider provider)
        {
            return value == null ? 0 : ((IConvertible)value).ToUInt32(provider);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(bool value)
        {
            return value ? 1u : 0u;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(char value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(sbyte value)
        {
            if (value < 0)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt32));
            Contract.EndContractBlock();
            return (uint)value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(byte value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(short value)
        {
            if (value < 0)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt32));
            Contract.EndContractBlock();
            return (uint)value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(ushort value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(int value)
        {
            if (value < 0)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt32));
            Contract.EndContractBlock();
            return (uint)value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(uint value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(long value)
        {
            if (value < 0 || value > uint.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt32));
            Contract.EndContractBlock();
            return (uint)value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(ulong value)
        {
            if (value > uint.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt32));
            Contract.EndContractBlock();
            return (uint)value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(float value)
        {
            return ToUInt32((double)value);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(double value)
        {
            if (value >= -0.5 && value < 4294967295.5)
            {
                uint result = (uint)value;
                double dif = value - result;
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (dif > 0.5 || dif == 0.5 && (result & 1) != 0)
                    result++;
                return result;
            }
            throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt32));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(decimal value)
        {
            return decimal.ToUInt32(decimal.Round(value, 0));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(string value)
        {
            if (value == null)
                return 0;
            return uint.Parse(value, CultureInfo.CurrentCulture);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return uint.Parse(value, NumberStyles.Integer, provider);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(DateTime value)
        {
            return ((IConvertible)value).ToUInt32(null);
        }

        public static long ToInt64(object value)
        {
            return value == null ? 0 : ((IConvertible)value).ToInt64(null);
        }

        public static long ToInt64(object value, IFormatProvider provider)
        {
            return value == null ? 0 : ((IConvertible)value).ToInt64(provider);
        }

        public static long ToInt64(bool value)
        {
            return value ? 1L : 0L;
        }

        public static long ToInt64(char value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static long ToInt64(sbyte value)
        {
            return value;
        }

        public static long ToInt64(byte value)
        {
            return value;
        }

        public static long ToInt64(short value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static long ToInt64(ushort value)
        {
            return value;
        }

        public static long ToInt64(int value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static long ToInt64(uint value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static long ToInt64(ulong value)
        {
            if (value > long.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int64));
            Contract.EndContractBlock();
            return (long)value;
        }

        public static long ToInt64(long value)
        {
            return value;
        }

        public static long ToInt64(float value)
        {
            return ToInt64((double)value);
        }

        public static long ToInt64(double value)
        {
            return checked((long)Math.Round(value));
        }

        public static long ToInt64(decimal value)
        {
            return decimal.ToInt64(decimal.Round(value, 0));
        }

        public static long ToInt64(string value)
        {
            if (value == null)
                return 0;
            return long.Parse(value, CultureInfo.CurrentCulture);
        }

        public static long ToInt64(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return long.Parse(value, NumberStyles.Integer, provider);
        }

        public static long ToInt64(DateTime value)
        {
            return ((IConvertible)value).ToInt64(null);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(object value)
        {
            return value == null ? 0 : ((IConvertible)value).ToUInt64(null);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(object value, IFormatProvider provider)
        {
            return value == null ? 0 : ((IConvertible)value).ToUInt64(provider);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(bool value)
        {
            return value ? 1uL : 0uL;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(char value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(sbyte value)
        {
            if (value < 0)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt64));
            Contract.EndContractBlock();
            return (ulong)value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(byte value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(short value)
        {
            if (value < 0)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt64));
            Contract.EndContractBlock();
            return (ulong)value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(ushort value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(int value)
        {
            if (value < 0)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt64));
            Contract.EndContractBlock();
            return (ulong)value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(uint value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(long value)
        {
            if (value < 0)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt64));
            Contract.EndContractBlock();
            return (ulong)value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(ulong value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(float value)
        {
            return ToUInt64((double)value);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(double value)
        {
            return checked((ulong)Math.Round(value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(decimal value)
        {
            return decimal.ToUInt64(decimal.Round(value, 0));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(string value)
        {
            if (value == null)
                return 0;
            return ulong.Parse(value, CultureInfo.CurrentCulture);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return ulong.Parse(value, NumberStyles.Integer, provider);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(DateTime value)
        {
            return ((IConvertible)value).ToUInt64(null);
        }

        public static float ToSingle(object value)
        {
            return value == null ? 0 : ((IConvertible)value).ToSingle(null);
        }

        public static float ToSingle(object value, IFormatProvider provider)
        {
            return value == null ? 0 : ((IConvertible)value).ToSingle(provider);
        }

        [CLSCompliant(false)]
        public static float ToSingle(sbyte value)
        {
            return value;
        }

        public static float ToSingle(byte value)
        {
            return value;
        }

        public static float ToSingle(char value)
        {
            return ((IConvertible)value).ToSingle(null);
        }

        public static float ToSingle(short value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static float ToSingle(ushort value)
        {
            return value;
        }

        public static float ToSingle(int value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static float ToSingle(uint value)
        {
            return value;
        }

        public static float ToSingle(long value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static float ToSingle(ulong value)
        {
            return value;
        }

        public static float ToSingle(float value)
        {
            return value;
        }

        public static float ToSingle(double value)
        {
            return (float)value;
        }

        public static float ToSingle(decimal value)
        {
            return (float)value;
        }

        public static float ToSingle(string value)
        {
            if (value == null)
                return 0;
            return float.Parse(value, CultureInfo.CurrentCulture);
        }

        public static float ToSingle(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return float.Parse(value, NumberStyles.Float | NumberStyles.AllowThousands, provider);
        }


        public static float ToSingle(bool value)
        {
            return value ? 1f : 0f;
        }

        public static float ToSingle(DateTime value)
        {
            return ((IConvertible)value).ToSingle(null);
        }

        public static double ToDouble(object value)
        {
            return value == null ? 0 : ((IConvertible)value).ToDouble(null);
        }

        public static double ToDouble(object value, IFormatProvider provider)
        {
            return value == null ? 0 : ((IConvertible)value).ToDouble(provider);
        }

        [CLSCompliant(false)]
        public static double ToDouble(sbyte value)
        {
            return value;
        }

        public static double ToDouble(byte value)
        {
            return value;
        }

        public static double ToDouble(short value)
        {
            return value;
        }

        public static double ToDouble(char value)
        {
            return ((IConvertible)value).ToDouble(null);
        }

        [CLSCompliant(false)]
        public static double ToDouble(ushort value)
        {
            return value;
        }

        public static double ToDouble(int value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static double ToDouble(uint value)
        {
            return value;
        }

        public static double ToDouble(long value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static double ToDouble(ulong value)
        {
            return value;
        }

        public static double ToDouble(float value)
        {
            return value;
        }

        public static double ToDouble(double value)
        {
            return value;
        }

        public static double ToDouble(decimal value)
        {
            return (double)value;
        }

        public static double ToDouble(string value)
        {
            if (value == null)
                return 0;
            return double.Parse(value, CultureInfo.CurrentCulture);
        }

        public static double ToDouble(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return double.Parse(value, NumberStyles.Float | NumberStyles.AllowThousands, provider);
        }

        public static double ToDouble(bool value)
        {
            return value ? 1d : 0d;
        }

        public static double ToDouble(DateTime value)
        {
            return ((IConvertible)value).ToDouble(null);
        }

        public static decimal ToDecimal(object value)
        {
            return value == null ? 0 : ((IConvertible)value).ToDecimal(null);
        }

        public static decimal ToDecimal(object value, IFormatProvider provider)
        {
            return value == null ? 0 : ((IConvertible)value).ToDecimal(provider);
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(sbyte value)
        {
            return value;
        }

        public static decimal ToDecimal(byte value)
        {
            return value;
        }

        public static decimal ToDecimal(char value)
        {
            return ((IConvertible)value).ToDecimal(null);
        }

        public static decimal ToDecimal(short value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(ushort value)
        {
            return value;
        }

        public static decimal ToDecimal(int value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(uint value)
        {
            return value;
        }

        public static decimal ToDecimal(long value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(ulong value)
        {
            return value;
        }

        public static decimal ToDecimal(float value)
        {
            return (decimal)value;
        }

        public static decimal ToDecimal(double value)
        {
            return (decimal)value;
        }

        public static decimal ToDecimal(string value)
        {
            if (value == null)
                return 0m;
            return decimal.Parse(value, CultureInfo.CurrentCulture);
        }

        public static decimal ToDecimal(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0m;
            return decimal.Parse(value, NumberStyles.Number, provider);
        }

        public static decimal ToDecimal(decimal value)
        {
            return value;
        }

        public static decimal ToDecimal(bool value)
        {
            return value ? 1m : 0m;
        }

        public static decimal ToDecimal(DateTime value)
        {
            return ((IConvertible)value).ToDecimal(null);
        }

        public static DateTime ToDateTime(DateTime value)
        {
            return value;
        }

        public static DateTime ToDateTime(object value)
        {
            return value == null ? DateTime.MinValue : ((IConvertible)value).ToDateTime(null);
        }

        public static DateTime ToDateTime(object value, IFormatProvider provider)
        {
            return value == null ? DateTime.MinValue : ((IConvertible)value).ToDateTime(provider);
        }

        public static DateTime ToDateTime(string value)
        {
            if (value == null)
                return new DateTime(0);
            return DateTime.Parse(value, CultureInfo.CurrentCulture);
        }

        public static DateTime ToDateTime(string value, IFormatProvider provider)
        {
            if (value == null)
                return new DateTime(0);
            return DateTime.Parse(value, provider);
        }

        [CLSCompliant(false)]
        public static DateTime ToDateTime(sbyte value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }

        public static DateTime ToDateTime(byte value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }

        public static DateTime ToDateTime(short value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }

        [CLSCompliant(false)]
        public static DateTime ToDateTime(ushort value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }

        public static DateTime ToDateTime(int value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }

        [CLSCompliant(false)]
        public static DateTime ToDateTime(uint value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }

        public static DateTime ToDateTime(long value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }

        [CLSCompliant(false)]
        public static DateTime ToDateTime(ulong value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }

        public static DateTime ToDateTime(bool value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }

        public static DateTime ToDateTime(char value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }

        public static DateTime ToDateTime(float value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }

        public static DateTime ToDateTime(double value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }

        public static DateTime ToDateTime(decimal value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }

        public static string ToString(object value)
        {
            return ToString(value, null);
        }

        public static string ToString(object value, IFormatProvider provider)
        {
            IConvertible ic = value as IConvertible;
            if (ic != null)
                return ic.ToString(provider);
            IFormattable formattable = value as IFormattable;
            if (formattable != null)
                return formattable.ToString(null, provider);
            return value == null ? string.Empty : value.ToString();
        }

        public static string ToString(bool value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString();
        }

        public static string ToString(bool value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        public static string ToString(char value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return char.ToString(value);
        }

        public static string ToString(char value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        [CLSCompliant(false)]
        public static string ToString(sbyte value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(CultureInfo.CurrentCulture);
        }

        [CLSCompliant(false)]
        public static string ToString(sbyte value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        public static string ToString(byte value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(CultureInfo.CurrentCulture);
        }

        public static string ToString(byte value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        public static string ToString(short value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(CultureInfo.CurrentCulture);
        }

        public static string ToString(short value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        [CLSCompliant(false)]
        public static string ToString(ushort value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(CultureInfo.CurrentCulture);
        }

        [CLSCompliant(false)]
        public static string ToString(ushort value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        public static string ToString(int value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(CultureInfo.CurrentCulture);
        }

        public static string ToString(int value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        [CLSCompliant(false)]
        public static string ToString(uint value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(CultureInfo.CurrentCulture);
        }

        [CLSCompliant(false)]
        public static string ToString(uint value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        public static string ToString(long value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(CultureInfo.CurrentCulture);
        }

        public static string ToString(long value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        [CLSCompliant(false)]
        public static string ToString(ulong value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(CultureInfo.CurrentCulture);
        }

        [CLSCompliant(false)]
        public static string ToString(ulong value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        public static string ToString(float value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(CultureInfo.CurrentCulture);
        }

        public static string ToString(float value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        public static string ToString(double value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(CultureInfo.CurrentCulture);
        }

        public static string ToString(double value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        public static string ToString(decimal value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(CultureInfo.CurrentCulture);
        }

        public static string ToString(decimal value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        public static string ToString(DateTime value)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString();
        }

        public static string ToString(DateTime value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return value.ToString(provider);
        }

        public static string ToString(string value)
        {
            Contract.Ensures(Contract.Result<string>() == value);
            return value;
        }

        public static string ToString(string value, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() == value);
            return value;
        }

        public static byte ToByte(string value, int fromBase)
        {
            if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_InvalidBase));
            Contract.EndContractBlock();
            int r = ParseNumbers.StringToInt(value, fromBase, ParseNumbers.IsTight | ParseNumbers.TreatAsUnsigned);
            if (r < byte.MinValue || r > byte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Byte));
            return (byte)r;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(string value, int fromBase)
        {
            if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
            {
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_InvalidBase));
            }
            Contract.EndContractBlock();
            int r = ParseNumbers.StringToInt(value, fromBase, ParseNumbers.IsTight | ParseNumbers.TreatAsI1);
            if (fromBase != 10 && r <= byte.MaxValue)
                return (sbyte)r;

            if (r < sbyte.MinValue || r > sbyte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte));
            return (sbyte)r;
        }

        public static short ToInt16(string value, int fromBase)
        {
            if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_InvalidBase));
            Contract.EndContractBlock();
            int r = ParseNumbers.StringToInt(value, fromBase, ParseNumbers.IsTight | ParseNumbers.TreatAsI2);
            if (fromBase != 10 && r <= ushort.MaxValue)
                return (short)r;
            if (r < short.MinValue || r > short.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int16));
            return (short)r;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(string value, int fromBase)
        {
            if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_InvalidBase));
            Contract.EndContractBlock();
            int r = ParseNumbers.StringToInt(value, fromBase, ParseNumbers.IsTight | ParseNumbers.TreatAsUnsigned);
            if (r < ushort.MinValue || r > ushort.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt16));
            return (ushort)r;
        }

        public static int ToInt32(string value, int fromBase)
        {
            if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_InvalidBase));
            Contract.EndContractBlock();
            return ParseNumbers.StringToInt(value, fromBase, ParseNumbers.IsTight);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(string value, int fromBase)
        {
            if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_InvalidBase));
            Contract.EndContractBlock();
            return (uint)ParseNumbers.StringToInt(value, fromBase, ParseNumbers.TreatAsUnsigned | ParseNumbers.IsTight);
        }

        public static long ToInt64(string value, int fromBase)
        {
            if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_InvalidBase));
            Contract.EndContractBlock();
            return ParseNumbers.StringToLong(value, fromBase, ParseNumbers.IsTight);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(string value, int fromBase)
        {
            if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_InvalidBase));
            Contract.EndContractBlock();
            return (ulong)ParseNumbers.StringToLong(value, fromBase, ParseNumbers.TreatAsUnsigned | ParseNumbers.IsTight);
        }

        [SecuritySafeCritical]
        public static string ToString(byte value, int toBase)
        {
            if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_InvalidBase));
            Contract.EndContractBlock();
            return ParseNumbers.IntToString(value, toBase, -1, ' ', ParseNumbers.PrintAsI1);
        }

        [SecuritySafeCritical]
        public static string ToString(short value, int toBase)
        {
            if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_InvalidBase));
            Contract.EndContractBlock();
            return ParseNumbers.IntToString(value, toBase, -1, ' ', ParseNumbers.PrintAsI2);
        }

        [SecuritySafeCritical]
        public static string ToString(int value, int toBase)
        {
            if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_InvalidBase));
            Contract.EndContractBlock();
            return ParseNumbers.IntToString(value, toBase, -1, ' ', 0);
        }

        [SecuritySafeCritical]
        public static string ToString(long value, int toBase)
        {
            if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_InvalidBase));
            Contract.EndContractBlock();
            return ParseNumbers.LongToString(value, toBase, -1, ' ', 0);
        }

        public static string ToBase64String(byte[] inArray)
        {
            if (inArray == null)
                throw new ArgumentNullException(nameof(inArray));
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.EndContractBlock();
            return ToBase64String(inArray, 0, inArray.Length, Base64FormattingOptions.None);
        }

        [ComVisible(false)]
        public static string ToBase64String(byte[] inArray, Base64FormattingOptions options)
        {
            if (inArray == null)
                throw new ArgumentNullException(nameof(inArray));
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.EndContractBlock();
            return ToBase64String(inArray, 0, inArray.Length, options);
        }

        public static string ToBase64String(byte[] inArray, int offset, int length)
        {
            return ToBase64String(inArray, offset, length, Base64FormattingOptions.None);
        }

        [SecuritySafeCritical]
        [ComVisible(false)]
        public static unsafe string ToBase64String(byte[] inArray, int offset, int length, Base64FormattingOptions options)
        {
            if (inArray == null)
                throw new ArgumentNullException(nameof(inArray));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_GenericPositive));
            if (options < Base64FormattingOptions.None || options > Base64FormattingOptions.InsertLineBreaks)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_EnumIllegalVal, (int)options));
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.EndContractBlock();

            int inArrayLength = inArray.Length;
            if (offset > inArrayLength - length)
                throw new ArgumentOutOfRangeException(nameof(offset), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_OffsetLength));

            if (inArrayLength == 0)
                return string.Empty;

            bool insertLineBreaks = (options == Base64FormattingOptions.InsertLineBreaks);
            int stringLength = ToBase64_CalculateAndValidateOutputLength(length, insertLineBreaks);

            string returnString = string.FastAllocateString(stringLength);
            fixed (char* outChars = returnString)
            {
                fixed (byte* inData = inArray)
                {
                    int j = ConvertToBase64Array(outChars, inData, offset, length, insertLineBreaks);
                    return returnString;
                }
            }
        }

        public static int ToBase64CharArray(byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut)
        {
            Contract.Ensures(Contract.Result<int>() >= 0);
            Contract.Ensures(Contract.Result<int>() <= outArray.Length);
            Contract.EndContractBlock();
            return ToBase64CharArray(inArray, offsetIn, length, outArray, offsetOut, Base64FormattingOptions.None);
        }

        [SecuritySafeCritical]
        [ComVisible(false)]
        public static unsafe int ToBase64CharArray(byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut, Base64FormattingOptions options)
        {
            if (inArray == null)
                throw new ArgumentNullException(nameof(inArray));
            if (outArray == null)
                throw new ArgumentNullException(nameof(outArray));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            if (offsetIn < 0)
                throw new ArgumentOutOfRangeException(nameof(offsetIn), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_GenericPositive));
            if (offsetOut < 0)
                throw new ArgumentOutOfRangeException(nameof(offsetOut), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_GenericPositive));
            if (options < Base64FormattingOptions.None || options > Base64FormattingOptions.InsertLineBreaks)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_EnumIllegalVal, (int)options));
            Contract.Ensures(Contract.Result<int>() >= 0);
            Contract.Ensures(Contract.Result<int>() <= outArray.Length);
            Contract.EndContractBlock();

            int inArrayLength = inArray.Length;
            if (offsetIn > inArrayLength - length)
                throw new ArgumentOutOfRangeException(nameof(offsetIn), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_OffsetLength));

            if (inArrayLength == 0)
                return 0;

            bool insertLineBreaks = (options == Base64FormattingOptions.InsertLineBreaks);
            int outArrayLength = outArray.Length;
            int numElementsToCopy = ToBase64_CalculateAndValidateOutputLength(length, insertLineBreaks);
            if (offsetOut > outArrayLength - numElementsToCopy)
                throw new ArgumentOutOfRangeException(nameof(offsetOut), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_OffsetOut));

            int retVal;
            fixed (char* outChars = &outArray[offsetOut])
            {
                fixed (byte* inData = inArray)
                {
                    retVal = ConvertToBase64Array(outChars, inData, offsetIn, length, insertLineBreaks);
                }
            }

            return retVal;
        }

        [SecurityCritical]
        private static unsafe int ConvertToBase64Array(char* outChars, byte* inData, int offset, int length, bool insertLineBreaks)
        {
            int lengthmod3 = length % 3;
            int calcLength = offset + (length - lengthmod3);
            int j = 0;
            int charcount = 0;

            fixed (char* base64 = _base64Table)
            {
                int i;
                for (i = offset; i < calcLength; i += 3)
                {
                    if (insertLineBreaks)
                    {
                        if (charcount == cBase64LineBreakPosition)
                        {
                            outChars[j++] = '\r';
                            outChars[j++] = '\n';
                            charcount = 0;
                        }
                        charcount += 4;
                    }
                    outChars[j] = base64[(inData[i] & 0xfc) >> 2];
                    outChars[j + 1] = base64[((inData[i] & 0x03) << 4) | ((inData[i + 1] & 0xf0) >> 4)];
                    outChars[j + 2] = base64[((inData[i + 1] & 0x0f) << 2) | ((inData[i + 2] & 0xc0) >> 6)];
                    outChars[j + 3] = base64[(inData[i + 2] & 0x3f)];
                    j += 4;
                }

                i = calcLength;
                if (insertLineBreaks && (lengthmod3 != 0) && (charcount == cBase64LineBreakPosition))
                {
                    outChars[j++] = '\r';
                    outChars[j++] = '\n';
                }

                switch (lengthmod3)
                {
                    case 2:
                        outChars[j] = base64[(inData[i] & 0xfc) >> 2];
                        outChars[j + 1] = base64[((inData[i] & 0x03) << 4) | ((inData[i + 1] & 0xf0) >> 4)];
                        outChars[j + 2] = base64[(inData[i + 1] & 0x0f) << 2];
                        outChars[j + 3] = base64[64];
                        j += 4;
                        break;
                    case 1:
                        outChars[j] = base64[(inData[i] & 0xfc) >> 2];
                        outChars[j + 1] = base64[(inData[i] & 0x03) << 4];
                        outChars[j + 2] = base64[64];
                        outChars[j + 3] = base64[64];
                        j += 4;
                        break;
                }
            }

            return j;
        }

        private static int ToBase64_CalculateAndValidateOutputLength(int inputLength, bool insertLineBreaks)
        {
            long outlen = (long)inputLength / 3 * 4;
            outlen += (inputLength % 3) != 0 ? 4 : 0;
            if (outlen == 0)
                return 0;

            if (insertLineBreaks)
            {
                long newLines = outlen / cBase64LineBreakPosition;
                if (outlen % cBase64LineBreakPosition == 0)
                    --newLines;
                outlen += newLines * 2;
            }
            if (outlen > int.MaxValue)
                throw new OutOfMemoryException();
            return (int)outlen;
        }

        [SecuritySafeCritical]
        public static byte[] FromBase64String(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            Contract.EndContractBlock();
            unsafe
            {
                fixed (char* sPtr = s)
                {
                    return FromBase64CharPtr(sPtr, s.Length);
                }
            }
        }

        [SecuritySafeCritical]
        public static byte[] FromBase64CharArray(char[] inArray, int offset, int length)
        {
            if (inArray == null)
                throw new ArgumentNullException(nameof(inArray));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_GenericPositive));
            if (offset > inArray.Length - length)
                throw new ArgumentOutOfRangeException(nameof(offset), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_OffsetLength));
            Contract.EndContractBlock();
            unsafe
            {
                fixed (char* inArrayPtr = inArray)
                {
                    return FromBase64CharPtr(inArrayPtr + offset, length);
                }
            }
        }

        [SecurityCritical]
        private static unsafe byte[] FromBase64CharPtr(char* inputPtr, int inputLength)
        {
            Contract.Assert(0 <= inputLength);
            while (inputLength > 0)
            {
                int lastChar = inputPtr[inputLength - 1];
                if (lastChar != (int)' ' && lastChar != (int)'\n' && lastChar != (int)'\r' && lastChar != (int)'\t')
                    break;
                inputLength--;
            }
            int resultLength = FromBase64_ComputeResultLength(inputPtr, inputLength);
            Contract.Assert(0 <= resultLength);
            byte[] decodedBytes = new byte[resultLength];
            int actualResultLength;
            fixed (byte* decodedBytesPtr = decodedBytes)
                actualResultLength = FromBase64_Decode(inputPtr, inputLength, decodedBytesPtr, resultLength);
            return decodedBytes;
        }

        [SecurityCritical]
        private static unsafe int FromBase64_Decode(char* startInputPtr, int inputLength, byte* startDestPtr, int destLength)
        {
            const uint intA = (uint)'A';
            const uint inta = (uint)'a';
            const uint int0 = (uint)'0';
            const uint intEq = (uint)'=';
            const uint intPlus = (uint)'+';
            const uint intSlash = (uint)'/';
            const uint intSpace = (uint)' ';
            const uint intTab = (uint)'\t';
            const uint intNLn = (uint)'\n';
            const uint intCRt = (uint)'\r';
            const uint intAtoZ = (uint)('Z' - 'A'); // = ('z' - 'a')
            const uint int0to9 = (uint)('9' - '0');

            char* inputPtr = startInputPtr;
            byte* destPtr = startDestPtr;
            char* endInputPtr = inputPtr + inputLength;
            byte* endDestPtr = destPtr + destLength;

            uint currCode;
            uint currBlockCodes = 0x000000FFu;

            unchecked
            {
                while (true)
                {
                    if (inputPtr >= endInputPtr)
                        goto _AllInputConsumed;
                    currCode = (uint)(*inputPtr);
                    inputPtr++;

                    if (currCode - intA <= intAtoZ)
                        currCode -= intA;
                    else if (currCode - inta <= intAtoZ)
                        currCode -= (inta - 26u);
                    else if (currCode - int0 <= int0to9)
                        currCode -= (int0 - 52u);
                    else
                    {
                        switch (currCode)
                        {
                            case intPlus:
                                currCode = 62u;
                                break;
                            case intSlash:
                                currCode = 63u;
                                break;
                            case intCRt:
                            case intNLn:
                            case intSpace:
                            case intTab:
                                continue;
                            case intEq:
                                goto _EqualityCharEncountered;
                            default:
                                throw new FormatException(__Resources.GetResourceString(__Resources.Format_BadBase64Char));
                        }
                    }

                    currBlockCodes = (currBlockCodes << 6) | currCode;
                    if ((currBlockCodes & 0x80000000u) != 0u)
                    {
                        if ((int)(endDestPtr - destPtr) < 3)
                            return -1;
                        *(destPtr) = (byte)(currBlockCodes >> 16);
                        *(destPtr + 1) = (byte)(currBlockCodes >> 8);
                        *(destPtr + 2) = (byte)(currBlockCodes);
                        destPtr += 3;
                        currBlockCodes = 0x000000FFu;
                    }
                }
            }

            _EqualityCharEncountered:

            Contract.Assert(currCode == intEq);

            if (inputPtr == endInputPtr)
            {
                currBlockCodes <<= 6;
                if ((currBlockCodes & 0x80000000u) == 0u)
                    throw new FormatException(__Resources.GetResourceString(__Resources.Format_BadBase64CharArrayLength));
                if ((int)(endDestPtr - destPtr) < 2)
                    return -1;
                *(destPtr++) = (byte)(currBlockCodes >> 16);
                *(destPtr++) = (byte)(currBlockCodes >> 8);
                currBlockCodes = 0x000000FFu;
            }
            else
            {
                while (inputPtr < (endInputPtr - 1))
                {
                    int lastChar = *(inputPtr);
                    if (lastChar != (int)' ' && lastChar != (int)'\n' && lastChar != (int)'\r' && lastChar != (int)'\t')
                        break;
                    inputPtr++;
                }
                if (inputPtr == (endInputPtr - 1) && *(inputPtr) == '=')
                {
                    currBlockCodes <<= 12;
                    if ((currBlockCodes & 0x80000000u) == 0u)
                        throw new FormatException(__Resources.GetResourceString(__Resources.Format_BadBase64CharArrayLength));
                    if ((int)(endDestPtr - destPtr) < 1)
                        return -1;

                    *(destPtr++) = (byte)(currBlockCodes >> 16);
                    currBlockCodes = 0x000000FFu;

                }
                else
                    throw new FormatException(__Resources.GetResourceString(__Resources.Format_BadBase64Char));
            }

            _AllInputConsumed:

            if (currBlockCodes != 0x000000FFu)
                throw new FormatException(__Resources.GetResourceString(__Resources.Format_BadBase64CharArrayLength));
            return (int)(destPtr - startDestPtr);
        }

        [SecurityCritical]
        private static unsafe int FromBase64_ComputeResultLength(char* inputPtr, int inputLength)
        {
            const uint intEq = (uint)'=';
            const uint intSpace = (uint)' ';
            Contract.Assert(0 <= inputLength);

            char* inputEndPtr = inputPtr + inputLength;
            int usefulInputLength = inputLength;
            int padding = 0;
            while (inputPtr < inputEndPtr)
            {
                uint c = (uint)(*inputPtr);
                inputPtr++;

                if (c <= intSpace)
                    usefulInputLength--;
                else if (c == intEq)
                {
                    usefulInputLength--;
                    padding++;
                }
            }
            Contract.Assert(0 <= usefulInputLength);
            Contract.Assert(0 <= padding);

            if (padding != 0)
            {
                if (padding == 1)
                    padding = 2;
                else if (padding == 2)
                    padding = 1;
                else
                    throw new FormatException(__Resources.GetResourceString(__Resources.Format_BadBase64Char));
            }
            return (usefulInputLength / 4) * 3 + padding;
        }
    }
}