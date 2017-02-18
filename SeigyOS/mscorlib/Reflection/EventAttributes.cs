using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum EventAttributes
    {
        None = 0x0000,
        SpecialName = 0x0200,
        ReservedMask = 0x0400,
        // ReSharper disable once InconsistentNaming
        RTSpecialName = 0x0400,
    }
}