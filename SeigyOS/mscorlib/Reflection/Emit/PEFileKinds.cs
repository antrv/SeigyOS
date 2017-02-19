using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [Serializable]
    [ComVisible(true)]
    // ReSharper disable once InconsistentNaming
    public enum PEFileKinds
    {
        Dll = 0x0001,
        ConsoleApplication = 0x0002,
        WindowApplication = 0x0003,
    }
}