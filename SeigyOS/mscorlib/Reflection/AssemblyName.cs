using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_AssemblyName))]
    [ComVisible(true)]
    public sealed class AssemblyName: _AssemblyName, ICloneable, ISerializable, IDeserializationCallback
    {
        // TODO: members
    }
}