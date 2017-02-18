using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_ConstructorInfo))]
    [PermissionSetAttribute(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    [ComVisible(true)]
    public abstract class ConstructorInfo: MethodBase, _ConstructorInfo
    {
        // TODO: members
    }
}