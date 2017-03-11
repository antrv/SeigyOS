using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    // ReSharper disable once InconsistentNaming
    public sealed class DBNull: ISerializable, IConvertible
    {
        public static readonly DBNull Value = new DBNull();

        private DBNull()
        {
        }

        private DBNull(SerializationInfo info, StreamingContext context)
        {
            throw new NotSupportedException(__Resources.GetResourceString(__Resources.NotSupported_DBNullSerial));
        }

        [SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            UnitySerializationHolder.GetUnitySerializationInfo(info, UnitySerializationHolder.NullUnity, null, null);
        }

        public override string ToString()
        {
            return string.Empty;
        }

        public string ToString(IFormatProvider provider)
        {
            return string.Empty;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.DBNull;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromDBNull));
        }

        object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType((IConvertible)this, type, provider);
        }
    }
}