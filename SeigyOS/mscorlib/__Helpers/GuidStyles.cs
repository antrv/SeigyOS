namespace System.__Helpers
{
    [Flags]
    internal enum GuidStyles
    {
        None = 0x00000000,
        AllowParenthesis = 0x00000001,
        AllowBraces = 0x00000002,
        AllowDashes = 0x00000004,
        AllowHexPrefix = 0x00000008,
        RequireParenthesis = 0x00000010,
        RequireBraces = 0x00000020,
        RequireDashes = 0x00000040,
        RequireHexPrefix = 0x00000080,

        HexFormat = RequireBraces | RequireHexPrefix,
        NumberFormat = None,
        DigitFormat = RequireDashes,
        BraceFormat = RequireBraces | RequireDashes,
        ParenthesisFormat = RequireParenthesis | RequireDashes,

        Any = AllowParenthesis | AllowBraces | AllowDashes | AllowHexPrefix,
    }
}