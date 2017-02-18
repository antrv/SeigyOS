using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [ComVisible(true)]
    public enum ImageFileMachine
    {
        I386 = 0x014c,
        // ReSharper disable once InconsistentNaming
        IA64 = 0x0200,
        // ReSharper disable once InconsistentNaming
        AMD64 = 0x8664,
        // ReSharper disable once InconsistentNaming
        ARM = 0x01c4,
    }
}