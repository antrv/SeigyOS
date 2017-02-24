using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
    [Serializable]
    [ComVisible(true)]
    public struct RuntimeMethodHandle: ISerializable
    {
        private IRuntimeMethodInfo _value;

        // ISerializable interface
        [SecurityCritical] // auto-generated
        private RuntimeMethodHandle(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            Contract.EndContractBlock();

            MethodBase m = (MethodBase)info.GetValue("MethodObj", typeof(MethodBase));

            _value = m.MethodHandle.m_value;

            if (_value == null)
                throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
        }

        [SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            Contract.EndContractBlock();

            if (_value == null)
                throw new SerializationException(Environment.GetResourceString("Serialization_InvalidFieldState"));

            // This is either a RuntimeMethodInfo or a RuntimeConstructorInfo
            MethodBase methodInfo = RuntimeType.GetMethodBase(_value);

            info.AddValue("MethodObj", methodInfo, typeof(MethodBase));
        }

        public IntPtr Value
        {
            [SecurityCritical]
            get
            {
                return _value != null ? _value.Value.Value : IntPtr.Zero;
            }
        }

        [SecuritySafeCritical]
        public override int GetHashCode()
        {
            return ValueType.GetHashCodeOfPtr(Value);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public override bool Equals(object obj)
        {
            if (!(obj is RuntimeMethodHandle))
                return false;

            RuntimeMethodHandle handle = (RuntimeMethodHandle)obj;

            return handle.Value == Value;
        }

        public static bool operator ==(RuntimeMethodHandle left, RuntimeMethodHandle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RuntimeMethodHandle left, RuntimeMethodHandle right)
        {
            return !left.Equals(right);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public bool Equals(RuntimeMethodHandle handle)
        {
            return handle.Value == Value;
        }

        [SecurityCritical]
        public IntPtr GetFunctionPointer()
        {
            IntPtr ptr = GetFunctionPointer(EnsureNonNullMethodInfo(_value).Value);
            GC.KeepAlive(_value);
            return ptr;
        }
    }
}