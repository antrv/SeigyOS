using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [ComVisible(true)]
    public enum ProcessorArchitecture
    {
        None = 0x0000,
        // ReSharper disable once InconsistentNaming
        MSIL = 0x0001,
        X86 = 0x0002,
        // ReSharper disable once InconsistentNaming
        IA64 = 0x0003,
        Amd64 = 0x0004,
        Arm = 0x0005
    }
}