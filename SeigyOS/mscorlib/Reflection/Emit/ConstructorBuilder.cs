using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [HostProtection(MayLeakOnAbort = true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_ConstructorBuilder))]
    [ComVisible(true)]
    public sealed class ConstructorBuilder: ConstructorInfo, _ConstructorBuilder
    {
        // TODO: members
    }
}