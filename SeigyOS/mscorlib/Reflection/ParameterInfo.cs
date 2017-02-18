using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_ParameterInfo))]
    [ComVisible(true)]
    public class ParameterInfo: _ParameterInfo, ICustomAttributeProvider, IObjectReference
    {
        // TODO: members
    }
}