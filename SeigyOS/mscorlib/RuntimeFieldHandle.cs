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
    public struct RuntimeFieldHandle: ISerializable
    {
        private IRuntimeFieldInfo m_ptr;

        public IntPtr Value
        {
            [SecurityCritical]
            get
            {
                return m_ptr != null ? m_ptr.Value.Value : IntPtr.Zero;
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
            if (!(obj is RuntimeFieldHandle))
                return false;

            RuntimeFieldHandle handle = (RuntimeFieldHandle)obj;

            return handle.Value == Value;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public bool Equals(RuntimeFieldHandle handle)
        {
            return handle.Value == Value;
        }

        public static bool operator ==(RuntimeFieldHandle left, RuntimeFieldHandle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RuntimeFieldHandle left, RuntimeFieldHandle right)
        {
            return !left.Equals(right);
        }

        [SecurityCritical]
        private RuntimeFieldHandle(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            Contract.EndContractBlock();

            FieldInfo f = (RuntimeFieldInfo)info.GetValue("FieldObj", typeof(RuntimeFieldInfo));

            if (f == null)
                throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));

            m_ptr = f.FieldHandle.m_ptr;

            if (m_ptr == null)
                throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
        }

        [SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            Contract.EndContractBlock();

            if (m_ptr == null)
                throw new SerializationException(Environment.GetResourceString("Serialization_InvalidFieldState"));

            RuntimeFieldInfo fldInfo = (RuntimeFieldInfo)RuntimeType.GetFieldInfo(GetRuntimeFieldInfo());

            info.AddValue("FieldObj", fldInfo, typeof(RuntimeFieldInfo));
        }
    }
}