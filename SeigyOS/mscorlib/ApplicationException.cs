using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class ApplicationException: Exception
    {
        public ApplicationException()
            : base(__Resources.GetResourceString(__Resources.Arg_ApplicationException))
        {
            HResult = __HResults.COR_E_APPLICATION;
        }

        public ApplicationException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_APPLICATION;
        }

        public ApplicationException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_APPLICATION;
        }

        protected ApplicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}