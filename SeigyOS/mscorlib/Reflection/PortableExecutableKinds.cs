using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum PortableExecutableKinds
    {
        NotAPortableExecutableImage = 0x0,
        // ReSharper disable once InconsistentNaming
        ILOnly = 0x1,
        Required32Bit = 0x2,
        // ReSharper disable once InconsistentNaming
        PE32Plus = 0x4,
        Unmanaged32Bit = 0x8,

        [ComVisible(false)]
        Preferred32Bit = 0x10,
    }
}