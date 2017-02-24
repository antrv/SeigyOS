using System.Diagnostics.Contracts;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
    [Serializable()]
    [ComVisible(true)]
    public struct RuntimeTypeHandle: ISerializable
    {
        private RuntimeType _type;

        public static bool operator ==(RuntimeTypeHandle left, object right)
        {
            return left.Equals(right);
        }

        public static bool operator ==(object left, RuntimeTypeHandle right)
        {
            return right.Equals(left);
        }

        public static bool operator !=(RuntimeTypeHandle left, object right)
        {
            return !left.Equals(right);
        }

        public static bool operator !=(object left, RuntimeTypeHandle right)
        {
            return !right.Equals(left);
        }

        public override int GetHashCode()
        {
            return _type != null ? _type.GetHashCode() : 0;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public override bool Equals(object obj)
        {
            if (!(obj is RuntimeTypeHandle))
                return false;
            RuntimeTypeHandle handle = (RuntimeTypeHandle)obj;
            return handle._type == _type;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public bool Equals(RuntimeTypeHandle handle)
        {
            return handle._type == _type;
        }

        public IntPtr Value
        {
            [SecurityCritical]
            get
            {
                return _type != null ? _type.m_handle : IntPtr.Zero;
            }
        }

        [CLSCompliant(false)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public ModuleHandle GetModuleHandle()
        {
            return new ModuleHandle(GetModule(_type));
        }

        [SecurityCritical] // auto-generated
        private RuntimeTypeHandle(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            Contract.EndContractBlock();

            RuntimeType m = (RuntimeType)info.GetValue("TypeObj", typeof(RuntimeType));

            _type = m;

            if (_type == null)
                throw new SerializationException(__Resources.GetResourceString("Serialization_InsufficientState"));
        }

        [SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            Contract.EndContractBlock();

            if (_type == null)
                throw new SerializationException(__Resources.GetResourceString("Serialization_InvalidFieldState"));

            info.AddValue("TypeObj", _type, typeof(RuntimeType));
        }
    }
}
