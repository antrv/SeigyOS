using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_FieldInfo))]
    [PermissionSetAttribute(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    [ComVisible(true)]
    public abstract class FieldInfo: MemberInfo, _FieldInfo
    {
        // TODO: members
    }
}