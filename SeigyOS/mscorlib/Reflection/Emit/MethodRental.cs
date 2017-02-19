using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [HostProtection(MayLeakOnAbort = true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_MethodRental))]
    [ComVisible(true)]
    public sealed class MethodRental: _MethodRental
    {
        // TODO: members
    }
}