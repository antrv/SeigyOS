using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum MethodAttributes
    {
        MemberAccessMask = 0x0007,
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
        Final = 0x0020,
        Virtual = 0x0040,
        HideBySig = 0x0080,
        CheckAccessOnOverride = 0x0200,
        VtableLayoutMask = 0x0100,
        ReuseSlot = 0x0000,
        NewSlot = 0x0100,
        Abstract = 0x0400,
        SpecialName = 0x0800,
        PinvokeImpl = 0x2000,
        UnmanagedExport = 0x0008,
        // ReSharper disable once InconsistentNaming
        RTSpecialName = 0x1000,
        ReservedMask = 0xd000,
        HasSecurity = 0x4000,
        RequireSecObject = 0x8000,
    }
}