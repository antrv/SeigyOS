using System.Runtime.InteropServices;

namespace System.Threading
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("C460E2B4-E199-412a-8456-84DC3E4838C3")]
    [ComVisible(true)]
    public interface IObjectHandle
    {
        object Unwrap();
    }
}