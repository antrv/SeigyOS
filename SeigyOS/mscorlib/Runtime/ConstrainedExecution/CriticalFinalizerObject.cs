using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.ConstrainedExecution
{
    [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
    [ComVisible(true)]
    public abstract class CriticalFinalizerObject
    {
        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected CriticalFinalizerObject()
        {
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        ~CriticalFinalizerObject()
        {
        }
    }
}