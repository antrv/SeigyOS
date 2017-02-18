using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Assembly))]
    [ComVisible(true)]
    [PermissionSetAttribute(SecurityAction.InheritanceDemand, Unrestricted = true)]
    public abstract class Assembly: _Assembly, IEvidenceFactory, ICustomAttributeProvider, ISerializable
    {
        // TODO: members
    }
}