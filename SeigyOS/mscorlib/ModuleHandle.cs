using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
    [ComVisible(true)]
    public struct ModuleHandle
    {
        public static readonly ModuleHandle EmptyHandle;

        private RuntimeModule m_ptr;


        public override int GetHashCode()
        {
            return m_ptr != null ? m_ptr.GetHashCode() : 0;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public override bool Equals(object obj)
        {
            if (!(obj is ModuleHandle))
                return false;

            ModuleHandle handle = (ModuleHandle)obj;

            return handle.m_ptr == m_ptr;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public bool Equals(ModuleHandle handle)
        {
            return handle.m_ptr == m_ptr;
        }

        public static bool operator ==(ModuleHandle left, ModuleHandle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ModuleHandle left, ModuleHandle right)
        {
            return !left.Equals(right);
        }

        public RuntimeTypeHandle GetRuntimeTypeHandleFromMetadataToken(int typeToken)
        {
            return ResolveTypeHandle(typeToken);
        }

        public RuntimeTypeHandle ResolveTypeHandle(int typeToken)
        {
            return new RuntimeTypeHandle(ResolveTypeHandleInternal(GetRuntimeModule(), typeToken, null, null));
        }

        public RuntimeTypeHandle ResolveTypeHandle(int typeToken, RuntimeTypeHandle[] typeInstantiationContext, RuntimeTypeHandle[] methodInstantiationContext)
        {
            return new RuntimeTypeHandle(ResolveTypeHandleInternal(GetRuntimeModule(), typeToken, typeInstantiationContext, methodInstantiationContext));
        }

        public RuntimeMethodHandle GetRuntimeMethodHandleFromMetadataToken(int methodToken)
        {
            return ResolveMethodHandle(methodToken);
        }

        public RuntimeMethodHandle ResolveMethodHandle(int methodToken)
        {
            return ResolveMethodHandle(methodToken, null, null);
        }

        public RuntimeMethodHandle ResolveMethodHandle(int methodToken, RuntimeTypeHandle[] typeInstantiationContext,
            RuntimeTypeHandle[] methodInstantiationContext)
        {
            return new RuntimeMethodHandle(ResolveMethodHandleInternal(GetRuntimeModule(), methodToken, typeInstantiationContext, methodInstantiationContext));
        }

        public RuntimeFieldHandle GetRuntimeFieldHandleFromMetadataToken(int fieldToken)
        {
            return ResolveFieldHandle(fieldToken);
        }

        public RuntimeFieldHandle ResolveFieldHandle(int fieldToken)
        {
            return new RuntimeFieldHandle(ResolveFieldHandleInternal(GetRuntimeModule(), fieldToken, null, null));
        }

        public RuntimeFieldHandle ResolveFieldHandle(int fieldToken, RuntimeTypeHandle[] typeInstantiationContext,
            RuntimeTypeHandle[] methodInstantiationContext)
        {
            return new RuntimeFieldHandle(ResolveFieldHandleInternal(GetRuntimeModule(), fieldToken, typeInstantiationContext, methodInstantiationContext));
        }
    }
}