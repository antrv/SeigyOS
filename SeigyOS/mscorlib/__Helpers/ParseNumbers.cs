using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security;

namespace System.__Helpers
{
    internal static class ParseNumbers
    {
        internal const int PrintAsI1 = 0x40;
        internal const int PrintAsI2 = 0x80;
        internal const int PrintAsI4 = 0x100;
        internal const int TreatAsUnsigned = 0x200;
        internal const int TreatAsI1 = 0x400;
        internal const int TreatAsI2 = 0x800;
        internal const int IsTight = 0x1000;
        internal const int NoSpace = 0x2000;

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern long StringToLong(string s, int radix, int flags, ref int currPos);

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int StringToInt(string s, int radix, int flags, ref int currPos);

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string IntToString(int l, int radix, int width, char paddingChar, int flags);

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string LongToString(long l, int radix, int width, char paddingChar, int flags);
    }
}