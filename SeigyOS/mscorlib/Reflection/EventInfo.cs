using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_EventInfo))]
    [PermissionSetAttribute(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    [ComVisible(true)]
    public abstract class EventInfo: MemberInfo, _EventInfo
    {
        // TODO: members
    }
}