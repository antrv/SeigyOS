using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Module))]
    [ComVisible(true)]
    [PermissionSetAttribute(SecurityAction.InheritanceDemand, Unrestricted = true)]
    public abstract class Module: _Module, ISerializable, ICustomAttributeProvider
    {
        // TODO: members
    }
}