using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;

namespace System.Threading
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Thread))]
    [ComVisible(true)]
    public sealed class Thread: CriticalFinalizerObject, _Thread
    {
        private readonly Delegate _delegate;

        [SecuritySafeCritical]
        public Thread(ThreadStart start)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        public Thread(ThreadStart start, int maxStackSize)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));
            if (0 > maxStackSize)
                throw new ArgumentOutOfRangeException(nameof(maxStackSize), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_NeedNonNegNum));
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        public Thread(ParameterizedThreadStart start)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        public Thread(ParameterizedThreadStart start, int maxStackSize)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));
            if (0 > maxStackSize)
                throw new ArgumentOutOfRangeException(nameof(maxStackSize), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_NeedNonNegNum));
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }

        [ComVisible(false)]
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public extern int ManagedThreadId
        {
            [ResourceExposure(ResourceScope.None)]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [MethodImpl(MethodImplOptions.InternalCall)]
            [SecuritySafeCritical]
            get;
        }

        [HostProtection(Synchronization = true, ExternalThreading = true)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Start()
        {
            throw new NotImplementedException();
        }

        [HostProtection(Synchronization = true, ExternalThreading = true)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Start(object parameter)
        {
            if (_delegate is ThreadStart)
                throw new InvalidOperationException(__Resources.GetResourceString(__Resources.InvalidOperation_ThreadWrongThreadStart));
            throw new NotImplementedException();
        }

        public ExecutionContext ExecutionContext
        {
            [SecuritySafeCritical]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            get
            {
                throw new NotImplementedException();
            }
        }

        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Abort(object stateInfo)
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Abort()
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public static void ResetAbort()
        {
            Thread thread = CurrentThread;
            if ((thread.ThreadState & ThreadState.AbortRequested) == 0)
                throw new ThreadStateException(__Resources.GetResourceString(__Resources.ThreadState_NoAbortRequested));
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        [Obsolete("Thread.Suspend has been deprecated.  Please use other classes in System.Threading, " +
                  "such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources. " +
                  "http://go.microsoft.com/fwlink/?linkid=14202", false)]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Suspend()
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        [Obsolete("Thread.Resume has been deprecated.  Please use other classes in System.Threading, " +
                  "such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources. " +
                  "http://go.microsoft.com/fwlink/?linkid=14202", false)]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Resume()
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Interrupt()
        {
            throw new NotImplementedException();
        }

        public ThreadPriority Priority
        {
            [SecuritySafeCritical]
            get
            {
                throw new NotImplementedException();
            }
            [SecuritySafeCritical]
            [HostProtection(SelfAffectingThreading = true)]
            set
            {
                throw new NotImplementedException();
            }
        }

        public extern bool IsAlive
        {
            [SecuritySafeCritical]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern bool IsThreadPoolThread
        {
            [SecuritySafeCritical]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [SecuritySafeCritical]
        [HostProtection(Synchronization = true, ExternalThreading = true)]
        public void Join()
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        [HostProtection(Synchronization = true, ExternalThreading = true)]
        public bool Join(int millisecondsTimeout)
        {
            throw new NotImplementedException();
        }

        [HostProtection(Synchronization = true, ExternalThreading = true)]
        public bool Join(TimeSpan timeout)
        {
            long tm = (long)timeout.TotalMilliseconds;
            if (tm < -1 || tm > (long)int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(timeout), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_NeedNonNegOrNegative1));
            return Join((int)tm);
        }

        [SecuritySafeCritical]
        public static void Sleep(int millisecondsTimeout)
        {
            throw new NotImplementedException();
        }

        public static void Sleep(TimeSpan timeout)
        {
            long tm = (long)timeout.TotalMilliseconds;
            if (tm < -1 || tm > (long)int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(timeout), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_NeedNonNegOrNegative1));
            Sleep((int)tm);
        }

        [SecuritySafeCritical]
        [HostProtection(Synchronization = true, ExternalThreading = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static void SpinWait(int iterations)
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        [HostProtection(Synchronization = true, ExternalThreading = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static bool Yield()
        {
            throw new NotImplementedException();
        }

        public static Thread CurrentThread
        {
            [SecuritySafeCritical]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            get
            {
                Contract.Ensures(Contract.Result<Thread>() != null);
                throw new NotImplementedException();
            }
        }

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        ~Thread()
        {
            throw new NotImplementedException();
        }

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public extern void DisableComObjectEagerCleanup();

        public bool IsBackground
        {
            [SecuritySafeCritical]
            get
            {
                throw new NotImplementedException();
            }
            [SecuritySafeCritical]
            [HostProtection(SelfAffectingThreading = true)]
            set
            {
                throw new NotImplementedException();
            }
        }

        public ThreadState ThreadState
        {
            [SecuritySafeCritical]
            get
            {
                throw new NotImplementedException();
            }
        }

        [Obsolete("The ApartmentState property has been deprecated.  Use GetApartmentState, SetApartmentState or TrySetApartmentState instead.", false)]
        public ApartmentState ApartmentState
        {
            [SecuritySafeCritical]
            get
            {
                throw new NotImplementedException();
            }

            [SecuritySafeCritical]
            [HostProtection(Synchronization = true, SelfAffectingThreading = true)]
            set
            {
                throw new NotImplementedException();
            }
        }

        [SecuritySafeCritical]
        public ApartmentState GetApartmentState()
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        [HostProtection(Synchronization = true, SelfAffectingThreading = true)]
        public bool TrySetApartmentState(ApartmentState state)
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        [HostProtection(Synchronization = true, SelfAffectingThreading = true)]
        public void SetApartmentState(ApartmentState state)
        {
            throw new NotImplementedException();
            bool result = true;
            if (!result)
                throw new InvalidOperationException(__Resources.GetResourceString("InvalidOperation_ApartmentStateSwitchFailed"));
        }

        [HostProtection(SharedState = true, ExternalThreading = true)]
        public static LocalDataStoreSlot AllocateDataSlot()
        {
            throw new NotImplementedException();
        }

        [HostProtection(SharedState = true, ExternalThreading = true)]
        public static LocalDataStoreSlot AllocateNamedDataSlot(string name)
        {
            throw new NotImplementedException();
        }

        [HostProtection(SharedState = true, ExternalThreading = true)]
        public static LocalDataStoreSlot GetNamedDataSlot(string name)
        {
            throw new NotImplementedException();
        }

        [HostProtection(SharedState = true, ExternalThreading = true)]
        public static void FreeNamedDataSlot(string name)
        {
            throw new NotImplementedException();
        }

        [HostProtection(SharedState = true, ExternalThreading = true)]
        [ResourceExposure(ResourceScope.AppDomain)]
        public static object GetData(LocalDataStoreSlot slot)
        {
            throw new NotImplementedException();
        }

        [HostProtection(SharedState = true, ExternalThreading = true)]
        [ResourceExposure(ResourceScope.AppDomain)]
        public static void SetData(LocalDataStoreSlot slot, object data)
        {
            throw new NotImplementedException();
        }

        public CultureInfo CurrentUICulture
        {
            get
            {
                Contract.Ensures(Contract.Result<CultureInfo>() != null);
                throw new NotImplementedException();
            }
            [SecuritySafeCritical]
            [HostProtection(ExternalThreading = true)]
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                Contract.EndContractBlock();
                throw new NotImplementedException();
            }
        }

        public CultureInfo CurrentCulture
        {
            get
            {
                Contract.Ensures(Contract.Result<CultureInfo>() != null);
                throw new NotImplementedException();
            }
            [SecuritySafeCritical]
            [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                Contract.EndContractBlock();
                throw new NotImplementedException();
            }
        }

        public static Context CurrentContext
        {
            [SecurityCritical]
            get
            {
                throw new NotImplementedException();
            }
        }

        public static IPrincipal CurrentPrincipal
        {
            [SecuritySafeCritical]
            get
            {
                throw new NotImplementedException();
            }

            [SecuritySafeCritical]
            [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
            set
            {
                throw new NotImplementedException();
            }
        }

        [SecuritySafeCritical]
        public static AppDomain GetDomain()
        {
            Contract.Ensures(Contract.Result<AppDomain>() != null);
            throw new NotImplementedException();
        }


        public static int GetDomainID()
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            [SecuritySafeCritical]
            [HostProtection(ExternalThreading = true)]
            set
            {
                throw new NotImplementedException();
            }
        }

        [SecuritySafeCritical]
        [HostProtection(Synchronization = true, ExternalThreading = true)]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern void BeginCriticalRegion();

        [SecuritySafeCritical]
        [HostProtection(Synchronization = true, ExternalThreading = true)]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static extern void EndCriticalRegion();

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern void BeginThreadAffinity();

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern void EndThreadAffinity();

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static byte VolatileRead(ref byte address)
        {
            byte ret = address;
            MemoryBarrier();
            return ret;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static short VolatileRead(ref short address)
        {
            short ret = address;
            MemoryBarrier();
            return ret;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int VolatileRead(ref int address)
        {
            int ret = address;
            MemoryBarrier();
            return ret;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static long VolatileRead(ref long address)
        {
            long ret = address;
            MemoryBarrier();
            return ret;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static sbyte VolatileRead(ref sbyte address)
        {
            sbyte ret = address;
            MemoryBarrier();
            return ret;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ushort VolatileRead(ref ushort address)
        {
            ushort ret = address;
            MemoryBarrier();
            return ret;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static uint VolatileRead(ref uint address)
        {
            uint ret = address;
            MemoryBarrier();
            return ret;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static IntPtr VolatileRead(ref IntPtr address)
        {
            IntPtr ret = address;
            MemoryBarrier();
            return ret;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static UIntPtr VolatileRead(ref UIntPtr address)
        {
            UIntPtr ret = address;
            MemoryBarrier();
            return ret;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ulong VolatileRead(ref ulong address)
        {
            ulong ret = address;
            MemoryBarrier();
            return ret;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static float VolatileRead(ref float address)
        {
            float ret = address;
            MemoryBarrier();
            return ret;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static double VolatileRead(ref double address)
        {
            double ret = address;
            MemoryBarrier();
            return ret;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static object VolatileRead(ref object address)
        {
            object ret = address;
            MemoryBarrier();
            return ret;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref byte address, byte value)
        {
            MemoryBarrier();
            address = value;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref short address, short value)
        {
            MemoryBarrier();
            address = value;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref int address, int value)
        {
            MemoryBarrier();
            address = value;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref long address, long value)
        {
            MemoryBarrier();
            address = value;
        }

        [CLSCompliant(false)]
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref sbyte address, sbyte value)
        {
            MemoryBarrier();
            address = value;
        }

        [CLSCompliant(false)]
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref ushort address, ushort value)
        {
            MemoryBarrier();
            address = value;
        }

        [CLSCompliant(false)]
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref uint address, uint value)
        {
            MemoryBarrier();
            address = value;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref IntPtr address, IntPtr value)
        {
            MemoryBarrier();
            address = value;
        }

        [CLSCompliant(false)]
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref UIntPtr address, UIntPtr value)
        {
            MemoryBarrier();
            address = value;
        }

        [CLSCompliant(false)]
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref ulong address, ulong value)
        {
            MemoryBarrier();
            address = value;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref float address, float value)
        {
            MemoryBarrier();
            address = value;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref double address, double value)
        {
            MemoryBarrier();
            address = value;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref object address, object value)
        {
            MemoryBarrier();
            address = value;
        }

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void MemoryBarrier();

        void _Thread.GetTypeInfoCount(out uint pcTInfo)
        {
            throw new NotImplementedException();
        }

        void _Thread.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
        {
            throw new NotImplementedException();
        }

        void _Thread.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint names, uint lcid, IntPtr rgDispId)
        {
            throw new NotImplementedException();
        }

        void _Thread.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo,
            IntPtr puArgErr)
        {
            throw new NotImplementedException();
        }
    }
}