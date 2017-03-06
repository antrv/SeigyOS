using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

namespace System.Threading
{
    public static class Interlocked
    {
        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static int Increment(ref int location)
        {
            return Add(ref location, 1);
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static long Increment(ref long location)
        {
            return Add(ref location, 1);
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static int Decrement(ref int location)
        {
            return Add(ref location, -1);
        }

        [ResourceExposure(ResourceScope.None)]
        public static long Decrement(ref long location)
        {
            return Add(ref location, -1);
        }

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public static extern int Exchange(ref int location1, int value);

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecuritySafeCritical]
        public static extern long Exchange(ref long location1, long value);

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecuritySafeCritical]
        public static extern float Exchange(ref float location1, float value);

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecuritySafeCritical]
        public static extern double Exchange(ref double location1, double value);

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public static extern object Exchange(ref object location1, object value);

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public static extern IntPtr Exchange(ref IntPtr location1, IntPtr value);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [ComVisible(false)]
        [SecuritySafeCritical]
        public static extern T Exchange<T>(ref T location1, T value)
            where T: class;

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public static extern int CompareExchange(ref int location1, int value, int comparand);

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecuritySafeCritical]
        public static extern long CompareExchange(ref long location1, long value, long comparand);

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecuritySafeCritical]
        public static extern float CompareExchange(ref float location1, float value, float comparand);

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecuritySafeCritical]
        public static extern double CompareExchange(ref double location1, double value, double comparand);

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public static extern object CompareExchange(ref object location1, object value, object comparand);

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public static extern IntPtr CompareExchange(ref IntPtr location1, IntPtr value, IntPtr comparand);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [Runtime.InteropServices.ComVisible(false)]
        [SecuritySafeCritical]
        public static extern T CompareExchange<T>(ref T location1, T value, T comparand)
            where T: class;

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static extern int Add(ref int location1, int value);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static extern long Add(ref long location1, long value);

        public static long Read(ref long location)
        {
            return CompareExchange(ref location, 0, 0);
        }

        public static void MemoryBarrier()
        {
            Thread.MemoryBarrier();
        }
    }
}