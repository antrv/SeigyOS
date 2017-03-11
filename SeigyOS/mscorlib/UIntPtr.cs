using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
    [Serializable]
    [CLSCompliant(false)]
    [ComVisible(true)]
    public struct UIntPtr: ISerializable
    {
        public static readonly UIntPtr Zero;

        [SecurityCritical]
        private unsafe void* _value;

        [SecuritySafeCritical]
        public unsafe UIntPtr(uint value)
        {
            _value = (void*)value;
        }

        [SecuritySafeCritical]
        public unsafe UIntPtr(ulong value)
        {
#if WIN32
            _value = (void*)checked((uint)value);
#else
            _value = (void*)value;
#endif
        }

        [SecurityCritical]
        [CLSCompliant(false)]
        public unsafe UIntPtr(void* value)
        {
            _value = value;
        }

        [SecurityCritical]
        private unsafe UIntPtr(SerializationInfo info, StreamingContext context)
        {
            ulong l = info.GetUInt64("value");
            if (Size == 4 && l > uint.MaxValue)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Serialization_InvalidPtrValue));
            _value = (void*)l;
        }

        [SecurityCritical]
        unsafe void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            Contract.EndContractBlock();
            info.AddValue("value", (ulong)_value);
        }

        [SecuritySafeCritical]
        public override unsafe bool Equals(object obj)
        {
            if (obj is UIntPtr)
            {
                return (_value == ((UIntPtr)obj)._value);
            }
            return false;
        }

        [SecuritySafeCritical]
        public override unsafe int GetHashCode()
        {
            return unchecked((int)((long)_value)) & 0x7fffffff;
        }

        [SecuritySafeCritical]
        public unsafe uint ToUInt32()
        {
#if WIN32
            return (uint)_value;
#else
            return checked((uint)_value);
#endif
        }

        [SecuritySafeCritical]
        public unsafe ulong ToUInt64()
        {
            return (ulong)_value;
        }

        [SecuritySafeCritical]
        public override unsafe string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);
#if WIN32
            return ((uint)_value).ToString(CultureInfo.InvariantCulture);
#else
            return ((ulong)_value).ToString(CultureInfo.InvariantCulture);
#endif
        }

        public static explicit operator UIntPtr(uint value)
        {
            return new UIntPtr(value);
        }

        public static explicit operator UIntPtr(ulong value)
        {
            return new UIntPtr(value);
        }

        [SecuritySafeCritical]
        public static unsafe explicit operator uint(UIntPtr value)
        {
#if WIN32
            return (uint)value._value;
#else
            return checked((uint)value._value);
#endif
        }

        [SecuritySafeCritical]
        public static unsafe explicit operator ulong(UIntPtr value)
        {
            return (ulong)value._value;
        }

        [SecurityCritical]
        [CLSCompliant(false)]
        public static unsafe explicit operator UIntPtr(void* value)
        {
            return new UIntPtr(value);
        }

        [SecurityCritical]
        [CLSCompliant(false)]
        public static unsafe explicit operator void*(UIntPtr value)
        {
            return value._value;
        }

        [SecuritySafeCritical]
        public static unsafe bool operator ==(UIntPtr value1, UIntPtr value2)
        {
            return value1._value == value2._value;
        }

        [SecuritySafeCritical]
        public static unsafe bool operator !=(UIntPtr value1, UIntPtr value2)
        {
            return value1._value != value2._value;
        }

        public static UIntPtr Add(UIntPtr pointer, int offset)
        {
            return pointer + offset;
        }

        public static UIntPtr operator +(UIntPtr pointer, int offset)
        {
#if WIN32
            return new UIntPtr(pointer.ToUInt32() + (uint)offset);
#else
            return new UIntPtr(pointer.ToUInt64() + (ulong)offset);
#endif
        }

        public static UIntPtr Subtract(UIntPtr pointer, int offset)
        {
            return pointer - offset;
        }

        public static UIntPtr operator -(UIntPtr pointer, int offset)
        {
#if WIN32
            return new UIntPtr(pointer.ToUInt32() - (uint)offset);
#else
            return new UIntPtr(pointer.ToUInt64() - (ulong)offset);
#endif
        }

        public static int Size
        {
            get
            {
#if WIN32
                return 4;
#else
                return 8;
#endif
            }
        }

        [SecuritySafeCritical]
        [CLSCompliant(false)]
        public unsafe void* ToPointer()
        {
            return _value;
        }
    }
}