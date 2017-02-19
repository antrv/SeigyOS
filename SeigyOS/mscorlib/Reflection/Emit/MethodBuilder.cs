using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [HostProtection(MayLeakOnAbort = true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_MethodBuilder))]
    [ComVisible(true)]
    public sealed class MethodBuilder: MethodInfo, _MethodBuilder
    {
        // TODO: members
    }
}