using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum AssemblyNameFlags
    {
        None = 0x0000,
        PublicKey = 0x0001,
        // ReSharper disable once InconsistentNaming
        EnableJITcompileOptimizer = 0x4000,
        // ReSharper disable once InconsistentNaming
        EnableJITcompileTracking = 0x8000,
        Retargetable = 0x0100,
    }
}