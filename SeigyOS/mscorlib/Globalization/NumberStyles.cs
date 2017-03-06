using System.Runtime.InteropServices;

namespace System.Globalization
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum NumberStyles
    {
        None = 0,
        AllowLeadingWhite = 1,
        AllowTrailingWhite = 2,
        AllowLeadingSign = 4,
        AllowTrailingSign = 8,
        AllowParentheses = 0x10,
        AllowDecimalPoint = 0x20,
        AllowThousands = 0x40,
        AllowExponent = 0x80,
        AllowCurrencySymbol = 0x100,
        AllowHexSpecifier = 0x200,
        Integer = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign,

        HexNumber = AllowLeadingWhite | AllowTrailingWhite | AllowHexSpecifier,

        Number = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign | AllowTrailingSign |
                 AllowDecimalPoint | AllowThousands,

        Float = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign |
                AllowDecimalPoint | AllowExponent,

        Currency = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign | AllowTrailingSign |
                   AllowParentheses | AllowDecimalPoint | AllowThousands | AllowCurrencySymbol,

        Any = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign | AllowTrailingSign |
              AllowParentheses | AllowDecimalPoint | AllowThousands | AllowCurrencySymbol | AllowExponent,

    }
}