using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class InvalidCastException: SystemException
    {
        public InvalidCastException()
            : base(__Resources.GetResourceString(__Resources.Arg_InvalidCastException))
        {
            HResult = __HResults.COR_E_INVALIDCAST;
        }

        public InvalidCastException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_INVALIDCAST;
        }

        public InvalidCastException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_INVALIDCAST;
        }

        protected InvalidCastException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public InvalidCastException(string message, int errorCode)
            : base(message)
        {
            SetErrorCode(errorCode);
        }
    }
}