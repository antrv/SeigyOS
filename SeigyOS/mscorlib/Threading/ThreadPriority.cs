using System.Runtime.InteropServices;

namespace System.Threading
{
    [Serializable]
    [ComVisible(true)]
    public enum ThreadPriority
    {
        Lowest = 0,
        BelowNormal = 1,
        Normal = 2,
        AboveNormal = 3,
        Highest = 4
    }
}