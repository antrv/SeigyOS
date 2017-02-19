using System.Runtime.InteropServices;
using System.Security;

namespace System
{
    [SecurityCritical]
    [Serializable]
    [ComVisible(true)]
    public delegate void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e);
}