using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
    [ComVisible(true)]
    public sealed class SerializationInfo
    {
        // TODO: members

        private IFormatterConverter _converter;

        [System.Security.SecuritySafeCritical]  // auto-generated
        public object GetValue(string name, Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            Contract.EndContractBlock();

            RuntimeType rt = type as RuntimeType;
            if (rt == null)
                throw new ArgumentException(__Resources.GetResourceString("Argument_MustBeRuntimeType"));

            Type foundType;
            object value = GetElement(name, out foundType);
            if (RemotingServices.IsTransparentProxy(value))
            {
                RealProxy proxy = RemotingServices.GetRealProxy(value);
                if (RemotingServices.ProxyCheckCast(proxy, rt))
                    return value;
            }
            else if (ReferenceEquals(foundType, type) || type.IsAssignableFrom(foundType) || value == null)
                return value;

            Contract.Assert(_converter != null, "[SerializationInfo.GetValue]m_converter!=null");

            return _converter.Convert(value, type);
        }

        public bool GetBoolean(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(bool)))
                return (bool)value;
            return _converter.ToBoolean(value);
        }

        public char GetChar(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(char)))
                return (char)value;
            return _converter.ToChar(value);
        }

        [CLSCompliant(false)]
        public sbyte GetSByte(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(sbyte)))
                return (sbyte)value;
            return _converter.ToSByte(value);
        }

        public byte GetByte(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(byte)))
                return (byte)value;
            return _converter.ToByte(value);
        }

        public short GetInt16(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(short)))
                return (short)value;
            return _converter.ToInt16(value);
        }

        [CLSCompliant(false)]
        public ushort GetUInt16(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(ushort)))
                return (ushort)value;
            return _converter.ToUInt16(value);
        }

        public int GetInt32(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(int)))
                return (int)value;
            return _converter.ToInt32(value);
        }

        [CLSCompliant(false)]
        public uint GetUInt32(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(uint)))
                return (uint)value;
            return _converter.ToUInt32(value);
        }

        public long GetInt64(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(long)))
                return (long)value;
            return _converter.ToInt64(value);
        }

        [CLSCompliant(false)]
        public ulong GetUInt64(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(ulong)))
                return (ulong)value;
            return _converter.ToUInt64(value);
        }

        public float GetSingle(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(float)))
                return (float)value;
            return _converter.ToSingle(value);
        }


        public double GetDouble(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(double)))
                return (double)value;
            return _converter.ToDouble(value);
        }

        public decimal GetDecimal(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(decimal)))
                return (decimal)value;
            return _converter.ToDecimal(value);
        }

        public DateTime GetDateTime(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(DateTime)))
                return (DateTime)value;
            return _converter.ToDateTime(value);
        }

        public string GetString(string name)
        {
            Type foundType;
            object value = GetElement(name, out foundType);
            if (ReferenceEquals(foundType, typeof(string)) || value == null)
                return (string)value;
            return _converter.ToString(value);
        }
    }
}