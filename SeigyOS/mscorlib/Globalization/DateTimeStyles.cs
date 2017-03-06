using System.Runtime.InteropServices;

namespace System.Globalization
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum DateTimeStyles
    {
        None = 0x00,
        AllowLeadingWhite = 0x01,
        AllowTrailingWhite = 0x02,
        AllowInnerWhite = 0x04,
        AllowWhiteSpaces = AllowLeadingWhite | AllowInnerWhite | AllowTrailingWhite,
        NoCurrentDateDefault = 0x08,
        AdjustToUniversal = 0x10,
        AssumeLocal = 0x20,
        AssumeUniversal = 0x40,
        RoundtripKind = 0x80,
    }
}