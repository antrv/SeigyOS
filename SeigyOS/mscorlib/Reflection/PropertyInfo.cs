using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_PropertyInfo))]
    [PermissionSetAttribute(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    [ComVisible(true)]
    public abstract class PropertyInfo: MemberInfo, _PropertyInfo
    {
        // TODO: members
    }
}