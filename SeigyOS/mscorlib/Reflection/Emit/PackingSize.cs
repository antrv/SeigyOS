using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [Serializable]
    [ComVisible(true)]
    public enum PackingSize
    {
        Unspecified = 0,
        Size1 = 1,
        Size2 = 2,
        Size4 = 4,
        Size8 = 8,
        Size16 = 16,
        Size32 = 32,
        Size64 = 64,
        Size128 = 128,
    }
}