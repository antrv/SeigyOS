using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
    [Serializable]
    [ComVisible(true)]
    public struct IntPtr: ISerializable
    {
        public static readonly IntPtr Zero;

        [SecurityCritical]
        private unsafe void* _value;

        [SecuritySafeCritical]
        [Pure]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal unsafe bool IsNull()
        {
            return _value == null;
        }

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public unsafe IntPtr(int value)
        {
            _value = (void*)value;
        }

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public unsafe IntPtr(long value)
        {
#if WIN32
            _value = (void*)checked((int)value);
#else
            _value = (void*)value;
#endif
        }

        [SecurityCritical]
        [CLSCompliant(false)]
        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public unsafe IntPtr(void* value)
        {
            _value = value;
        }

        [SecurityCritical]
        private unsafe IntPtr(SerializationInfo info, StreamingContext context)
        {
            long l = info.GetInt64("value");
            if (Size == 4 && (l > int.MaxValue || l < int.MinValue))
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Serialization_InvalidPtrValue));
            _value = (void*)l;
        }

        [SecurityCritical]
        unsafe void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            Contract.EndContractBlock();
            info.AddValue("value", (long)_value);
        }

        [SecuritySafeCritical]
        public override unsafe bool Equals(object obj)
        {
            return obj is IntPtr && _value == ((IntPtr)obj)._value;
        }

        [SecuritySafeCritical]
        public override unsafe int GetHashCode()
        {
            return unchecked((int)(long)_value);
        }

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public unsafe int ToInt32()
        {
#if WIN32
            return (int)_value;
#else
            long l = (long)_value;
            return checked((int)l);
#endif
        }

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public unsafe long ToInt64()
        {
            return (long)_value;
        }

        [SecuritySafeCritical]
        public unsafe override string ToString()
        {
#if WIN32
            return ((int)_value).ToString(CultureInfo.InvariantCulture);
#else
            return ((long)_value).ToString(CultureInfo.InvariantCulture);
#endif
        }

        [SecuritySafeCritical]
        public unsafe string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);
#if WIN32
            return ((int)_value).ToString(format, CultureInfo.InvariantCulture);
#else
            return ((long)_value).ToString(format, CultureInfo.InvariantCulture);
#endif
        }

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static explicit operator IntPtr(int value)
        {
            return new IntPtr(value);
        }

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static explicit operator IntPtr(long value)
        {
            return new IntPtr(value);
        }

        [SecurityCritical]
        [CLSCompliant(false)]
        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static unsafe explicit operator IntPtr(void* value)
        {
            return new IntPtr(value);
        }

        [SecuritySafeCritical]
        [CLSCompliant(false)]
        public static unsafe explicit operator void*(IntPtr value)
        {
            return value._value;
        }

        [SecuritySafeCritical]
        public static unsafe explicit operator int(IntPtr value)
        {
#if WIN32
            return (int)value._value;
#else
            long l = (long)value._value;
            return checked((int)l);
#endif
        }

        [SecuritySafeCritical]
        public static unsafe explicit operator long(IntPtr value)
        {
            return (long)value._value;
        }

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static unsafe bool operator ==(IntPtr value1, IntPtr value2)
        {
            return value1._value == value2._value;
        }

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static unsafe bool operator !=(IntPtr value1, IntPtr value2)
        {
            return value1._value != value2._value;
        }

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static IntPtr Add(IntPtr pointer, int offset)
        {
            return pointer + offset;
        }

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static IntPtr operator +(IntPtr pointer, int offset)
        {
#if WIN32
            return new IntPtr(pointer.ToInt32() + offset);
#else
            return new IntPtr(pointer.ToInt64() + offset);
#endif
        }

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static IntPtr Subtract(IntPtr pointer, int offset)
        {
            return pointer - offset;
        }

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static IntPtr operator -(IntPtr pointer, int offset)
        {
#if WIN32
            return new IntPtr(pointer.ToInt32() - offset);
#else
            return new IntPtr(pointer.ToInt64() - offset);
#endif
        }

        public static int Size
        {
            [Pure]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
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
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public unsafe void* ToPointer()
        {
            return _value;
        }
    }
}