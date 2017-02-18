using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
    [Serializable]
    [ComVisible(true)]
    public class UnhandledExceptionEventArgs: EventArgs
    {
        private readonly object _exception;
        private readonly bool _isTerminating;

        public UnhandledExceptionEventArgs(object exception, bool isTerminating)
        {
            _exception = exception;
            _isTerminating = isTerminating;
        }

        public object ExceptionObject
        {
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            get
            {
                return _exception;
            }
        }

        public bool IsTerminating
        {
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            get
            {
                return _isTerminating;
            }
        }
    }
}