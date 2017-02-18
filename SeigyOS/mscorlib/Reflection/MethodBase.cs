using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_MethodBase))]
    [PermissionSetAttribute(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    [ComVisible(true)]
    public abstract class MethodBase: MemberInfo, _MethodBase
    {
        // TODO: members
    }
}