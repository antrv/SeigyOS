using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [HostProtection(MayLeakOnAbort = true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_TypeBuilder))]
    [ComVisible(true)]
    public sealed class TypeBuilder: TypeInfo, _TypeBuilder
    {
        // TODO: members
    }
}