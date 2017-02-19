using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [HostProtection(MayLeakOnAbort = true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_FieldBuilder))]
    [ComVisible(true)]
    public sealed class FieldBuilder: FieldInfo, _FieldBuilder
    {
        // TODO: members
    }
}