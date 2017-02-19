using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [HostProtection(MayLeakOnAbort = true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_EventBuilder))]
    [ComVisible(true)]
    public sealed class EventBuilder: _EventBuilder
    {
        // TODO: members
    }
}