using System.Runtime.ConstrainedExecution;
using System.Runtime.Versioning;
using System.Security;

namespace System.Threading
{
    public static class Volatile
    {
        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static bool Read(ref bool location)
        {
            bool value = location;
            Thread.MemoryBarrier();
            return value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [CLSCompliant(false)]
        public static sbyte Read(ref sbyte location)
        {
            sbyte value = location;
            Thread.MemoryBarrier();
            return value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static byte Read(ref byte location)
        {
            byte value = location;
            Thread.MemoryBarrier();
            return value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static short Read(ref short location)
        {
            short value = location;
            Thread.MemoryBarrier();
            return value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [CLSCompliant(false)]
        public static ushort Read(ref ushort location)
        {
            ushort value = location;
            Thread.MemoryBarrier();
            return value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static int Read(ref int location)
        {
            int value = location;
            Thread.MemoryBarrier();
            return value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [CLSCompliant(false)]
        public static uint Read(ref uint location)
        {
            uint value = location;
            Thread.MemoryBarrier();
            return value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static long Read(ref long location)
        {
            return Interlocked.CompareExchange(ref location, 0, 0);
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [CLSCompliant(false)]
        [SecuritySafeCritical]
        public static ulong Read(ref ulong location)
        {
            unsafe
            {
                fixed (ulong* pLocation = &location)
                {
                    return (ulong)Interlocked.CompareExchange(ref *(long*)pLocation, 0, 0);
                }
            }
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static IntPtr Read(ref IntPtr location)
        {
            IntPtr value = location;
            Thread.MemoryBarrier();
            return value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [CLSCompliant(false)]
        public static UIntPtr Read(ref UIntPtr location)
        {
            UIntPtr value = location;
            Thread.MemoryBarrier();
            return value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static float Read(ref float location)
        {
            float value = location;
            Thread.MemoryBarrier();
            return value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static double Read(ref double location)
        {
            return Interlocked.CompareExchange(ref location, 0, 0);
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public static T Read<T>(ref T location) 
            where T: class
        {
            T value = location;
            Thread.MemoryBarrier();
            return value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static void Write(ref bool location, bool value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [CLSCompliant(false)]
        public static void Write(ref sbyte location, sbyte value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static void Write(ref byte location, byte value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static void Write(ref short location, short value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [CLSCompliant(false)]
        public static void Write(ref ushort location, ushort value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static void Write(ref int location, int value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [CLSCompliant(false)]
        public static void Write(ref uint location, uint value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static void Write(ref long location, long value)
        {
            Interlocked.Exchange(ref location, value);
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [CLSCompliant(false)]
        [SecuritySafeCritical] // contains unsafe code
        public static void Write(ref ulong location, ulong value)
        {
            unsafe
            {
                fixed (ulong* pLocation = &location)
                {
                    Interlocked.Exchange(ref *(long*)pLocation, (long)value);
                }
            }
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static void Write(ref IntPtr location, IntPtr value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [CLSCompliant(false)]
        public static void Write(ref UIntPtr location, UIntPtr value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static void Write(ref float location, float value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static void Write(ref double location, double value)
        {
            //
            Interlocked.Exchange(ref location, value);
        }

        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public static void Write<T>(ref T location, T value) 
            where T: class
        {
            Thread.MemoryBarrier();
            location = value;
        }
    }
}