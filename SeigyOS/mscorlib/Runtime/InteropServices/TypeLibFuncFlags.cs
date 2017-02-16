namespace System.Runtime.InteropServices
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum TypeLibFuncFlags
    {
        FRestricted = 0x0001,
        FSource = 0x0002,
        FBindable = 0x0004,
        FRequestEdit = 0x0008,
        FDisplayBind = 0x0010,
        FDefaultBind = 0x0020,
        FHidden = 0x0040,
        FUsesGetLastError = 0x0080,
        FDefaultCollelem = 0x0100,
        FUiDefault = 0x0200,
        FNonBrowsable = 0x0400,
        FReplaceable = 0x0800,
        FImmediateBind = 0x1000,
    }
}