using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum StreamingContextStates
    {
        CrossProcess = 0x01,
        CrossMachine = 0x02,
        File = 0x04,
        Persistence = 0x08,
        Remoting = 0x10,
        Other = 0x20,
        Clone = 0x40,
        CrossAppDomain = 0x80,
        All = 0xFF,
    }
}