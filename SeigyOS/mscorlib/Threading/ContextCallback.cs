using System.Runtime.InteropServices;
using System.Security;

namespace System.Threading
{
    [SecurityCritical]
    [ComVisible(true)]
    public delegate void ContextCallback(object state);
}