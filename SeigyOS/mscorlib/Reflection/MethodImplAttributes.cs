using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [ComVisible(true)]
    public enum MethodImplAttributes
    {
        CodeTypeMask = 0x0003,
        // ReSharper disable once InconsistentNaming
        IL = 0x0000,
        Native = 0x0001,
        // ReSharper disable once InconsistentNaming
        OPTIL = 0x0002,
        Runtime = 0x0003,
        ManagedMask = 0x0004,
        Unmanaged = 0x0004,
        Managed = 0x0000,
        ForwardRef = 0x0010,
        PreserveSig = 0x0080,
        InternalCall = 0x1000,
        Synchronized = 0x0020,
        NoInlining = 0x0008,

        [ComVisible(false)]
        AggressiveInlining = 0x0100,
        NoOptimization = 0x0040,
        MaxMethodImplVal = 0xFFFF,
    }
}