using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
    [ComVisible(true)]
    [Serializable]
    public class ThreadStateException: SystemException
    {
        public ThreadStateException()
            : base(__Resources.GetResourceString(__Resources.Arg_ThreadStateException))
        {
            HResult = __HResults.COR_E_THREADSTATE;
        }

        public ThreadStateException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_THREADSTATE;
        }

        public ThreadStateException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_THREADSTATE;
        }

        protected ThreadStateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}