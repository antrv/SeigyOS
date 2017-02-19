using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [HostProtection(MayLeakOnAbort = true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_EnumBuilder))]
    [ComVisible(true)]
    public sealed class EnumBuilder: TypeInfo, _EnumBuilder
    {
        // TODO: members
    }
}