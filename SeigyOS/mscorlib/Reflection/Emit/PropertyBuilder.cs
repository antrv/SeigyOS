using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [HostProtection(MayLeakOnAbort = true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_PropertyBuilder))]
    [ComVisible(true)]
    public sealed class PropertyBuilder: PropertyInfo, _PropertyBuilder
    {
        // TODO: members
    }
}