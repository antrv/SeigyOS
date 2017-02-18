using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum FieldAttributes
    {
        FieldAccessMask = 0x0007,
        PrivateScope = 0x0000,
        Private = 0x0001,
        // ReSharper disable once InconsistentNaming
        FamANDAssem = 0x0002,
        Assembly = 0x0003,
        Family = 0x0004,
        // ReSharper disable once InconsistentNaming
        FamORAssem = 0x0005,
        Public = 0x0006,
        Static = 0x0010,
        InitOnly = 0x0020,
        Literal = 0x0040,
        NotSerialized = 0x0080,
        SpecialName = 0x0200,
        PinvokeImpl = 0x2000,
        ReservedMask = 0x9500,
        // ReSharper disable once InconsistentNaming
        RTSpecialName = 0x0400,
        HasFieldMarshal = 0x1000,
        HasDefault = 0x8000,
        // ReSharper disable once InconsistentNaming
        HasFieldRVA = 0x0100,
    }
}