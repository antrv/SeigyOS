using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_LocalBuilder))]
    [ComVisible(true)]
    public sealed class LocalBuilder: LocalVariableInfo, _LocalBuilder
    {
        // TODO: members
    }
}