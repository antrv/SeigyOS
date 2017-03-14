using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class AccessViolationException: SystemException
    {
        public AccessViolationException()
            : base(__Resources.GetResourceString(__Resources.Arg_AccessViolationException))
        {
            HResult = __HResults.E_POINTER;
        }

        public AccessViolationException(string message)
            : base(message)
        {
            HResult = __HResults.E_POINTER;
        }

        public AccessViolationException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.E_POINTER;
        }

        protected AccessViolationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}