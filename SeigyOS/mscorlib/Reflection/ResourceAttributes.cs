using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum ResourceAttributes
    {
        Public = 0x0001,
        Private = 0x0002,
    }
}