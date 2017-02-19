using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [HostProtection(MayLeakOnAbort = true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_CustomAttributeBuilder))]
    [ComVisible(true)]
    public class CustomAttributeBuilder: _CustomAttributeBuilder
    {
        // TODO: members
    }
}