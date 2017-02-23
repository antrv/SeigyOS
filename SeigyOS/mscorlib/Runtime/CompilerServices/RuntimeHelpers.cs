using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

namespace System.Runtime.CompilerServices
{
    public static class RuntimeHelpers
    {
        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void InitializeArray(Array array, RuntimeFieldHandle fldHandle);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern object GetObjectValue(object obj);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _RunClassConstructor(RuntimeType type);

        public static void RunClassConstructor(RuntimeTypeHandle type)
        {
            _RunClassConstructor(type.GetRuntimeType());
        }

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _RunModuleConstructor(Reflection.RuntimeModule module);

        public static void RunModuleConstructor(ModuleHandle module)
        {
            _RunModuleConstructor(module.GetRuntimeModule());
        }

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static unsafe extern void _PrepareMethod(IRuntimeMethodInfo method, IntPtr* pInstantiation, int cInstantiation);

        [SecurityCritical]
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        internal static extern void _CompileMethod(IRuntimeMethodInfo method);

        // Simple (instantiation not required) method.
        [SecurityCritical]
        public static void PrepareMethod(RuntimeMethodHandle method)
        {
            unsafe
            {
                _PrepareMethod(method.GetMethodInfo(), null, 0);
            }
        }

        // Generic method or method with generic class with specific instantiation.
        [SecurityCritical]
        public static void PrepareMethod(RuntimeMethodHandle method, RuntimeTypeHandle[] instantiation)
        {
            unsafe
            {
                int length;
                IntPtr[] instantiationHandles = RuntimeTypeHandle.CopyRuntimeTypeHandles(instantiation, out length);
                fixed (IntPtr* pInstantiation = instantiationHandles)
                {
                    _PrepareMethod(method.GetMethodInfo(), pInstantiation, length);
                    GC.KeepAlive(instantiation);
                }
            }
        }

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void PrepareDelegate(Delegate d);

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void PrepareContractedDelegate(Delegate d);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int GetHashCode(object o);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public new static extern bool Equals(object o1, object o2);

        public static int OffsetToStringData
        {
            // This offset is baked in by string indexer intrinsic, so there is no harm
            // in getting it baked in here as well.
            [Versioning.NonVersionable]
            get
            {
                // Number of bytes from the address pointed to by a reference to
                // a String to the first 16-bit character in the String.  Skip 
                // over the MethodTable pointer, & String 
                // length.  Of course, the String reference points to the memory 
                // after the [....] block, so don't count that.  
                // This property allows C#'s fixed statement to work on Strings.
                // On 64 bit platforms, this should be 12 (8+4) and on 32 bit 8 (4+4).
#if WIN32
                return 8;
#else
                return 12;
#endif // WIN32
            }
        }

        // This method ensures that there is sufficient stack to execute the average Framework function.
        // If there is not enough stack, then it throws System.InsufficientExecutionStackException.
        // Note: this method is not part of the CER support, and is not to be confused with ProbeForSufficientStack
        // below.
        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static extern void EnsureSufficientExecutionStack();

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern void ProbeForSufficientStack();

        // This method is a marker placed immediately before a try clause to mark the corresponding catch and finally blocks as
        // constrained. There's no code here other than the probe because most of the work is done at JIT time when we spot a call to this routine.
        [SecurityCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void PrepareConstrainedRegions()
        {
            ProbeForSufficientStack();
        }

        // When we detect a CER with no calls, we can point the JIT to this non-probing version instead
        // as we don't need to probe.
        [SecurityCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        // ReSharper disable once InconsistentNaming
        public static void PrepareConstrainedRegionsNoOP()
        {
        }

        [SecurityCritical]
        public delegate void TryCode(object userData);

        [SecurityCritical]
        public delegate void CleanupCode(object userData, bool exceptionThrown);

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void ExecuteCodeWithGuaranteedCleanup(TryCode code, CleanupCode backoutCode, object userData);

        [SecurityCritical]
        [PrePrepareMethod]
        internal static void ExecuteBackoutCodeHelper(object backoutCode, object userData, bool exceptionThrown)
        {
            ((CleanupCode)backoutCode)(userData, exceptionThrown);
        }
    }
}