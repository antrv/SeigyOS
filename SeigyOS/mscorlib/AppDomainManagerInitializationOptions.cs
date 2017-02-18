using System.Runtime.InteropServices;

namespace System
{
    [Flags]
    [ComVisible(true)]
    public enum AppDomainManagerInitializationOptions
    {
        None = 0x0000,
        RegisterWithHost = 0x0001
    }
}