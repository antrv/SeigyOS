using System.Runtime.ConstrainedExecution;
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

        public static void RunClassConstructor(RuntimeTypeHandle type)
        {
            throw new NotImplementedException();
        }

        public static void RunModuleConstructor(ModuleHandle module)
        {
            throw new NotImplementedException();
        }

        [SecurityCritical]
        public static void PrepareMethod(RuntimeMethodHandle method)
        {
            throw new NotImplementedException();
        }

        [SecurityCritical]
        public static void PrepareMethod(RuntimeMethodHandle method, RuntimeTypeHandle[] instantiation)
        {
            throw new NotImplementedException();
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
            get
            {
#if WIN32
                return 8;
#else
                return 12;
#endif
            }
        }

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
    }
}