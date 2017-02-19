using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [HostProtection(MayLeakOnAbort = true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_AssemblyBuilder))]
    [ComVisible(true)]
    public sealed class AssemblyBuilder: Assembly, _AssemblyBuilder
    {
        // TODO: members
    }
}