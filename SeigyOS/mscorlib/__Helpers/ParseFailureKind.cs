namespace System.__Helpers
{
    internal enum ParseFailureKind
    {
        None = 0,
        ArgumentNull = 1,
        Format = 2,
        FormatWithParameter = 3,
        NativeException = 4,
        FormatWithInnerException = 5
    }
}