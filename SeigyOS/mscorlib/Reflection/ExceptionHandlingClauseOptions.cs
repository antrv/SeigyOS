using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Flags]
    [ComVisible(true)]
    public enum ExceptionHandlingClauseOptions
    {
        Clause = 0x0,
        Filter = 0x1,
        Finally = 0x2,
        Fault = 0x4,
    }
}