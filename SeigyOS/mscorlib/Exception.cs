using System.Runtime.InteropServices;

namespace System
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Exception))]
    [Serializable]
    [ComVisible(true)]
    public class Exception: ISerializable, _Exception
    {
        // TODO: members
    }
}