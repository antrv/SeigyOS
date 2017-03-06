using System.Runtime.InteropServices;

namespace System.Globalization
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum CultureTypes
    {
        NeutralCultures = 0x0001,
        SpecificCultures = 0x0002,
        InstalledWin32Cultures = 0x0004,
        AllCultures = NeutralCultures | SpecificCultures | InstalledWin32Cultures,
        UserCustomCulture = 0x0008,
        ReplacementCultures = 0x0010,

        [Obsolete("This value has been deprecated. Please use other values in CultureTypes.")]
        WindowsOnlyCultures = 0x0020,

        [Obsolete("This value has been deprecated. Please use other values in CultureTypes.")]
        FrameworkCultures = 0x0040,
    }
}