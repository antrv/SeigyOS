using System.Runtime.InteropServices;

namespace System
{
    [Serializable]
    [AttributeUsageAttribute(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Attribute))]
    [ComVisible(true)]
    public abstract class Attribute : _Attribute
    {
        // TODO: Base types
        // TODO: Members
    }
}