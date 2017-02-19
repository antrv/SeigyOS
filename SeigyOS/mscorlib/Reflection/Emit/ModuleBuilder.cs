using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [HostProtection(MayLeakOnAbort = true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_ModuleBuilder))]
    [ComVisible(true)]
    public class ModuleBuilder: Module, _ModuleBuilder
    {
        // TODO: members
    }
}