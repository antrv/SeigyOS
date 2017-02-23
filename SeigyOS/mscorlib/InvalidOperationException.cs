using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class InvalidOperationException: SystemException
    {
        public InvalidOperationException()
            : base(__Resources.GetResourceString("Arg_InvalidOperationException"))
        {
            HResult = __HResults.COR_E_INVALIDOPERATION;
        }

        public InvalidOperationException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_INVALIDOPERATION;
        }

        public InvalidOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_INVALIDOPERATION;
        }

        protected InvalidOperationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}