using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class FormatException: SystemException
    {
        public FormatException()
            : base(__Resources.GetResourceString(__Resources.Arg_FormatException))
        {
            HResult = __HResults.COR_E_FORMAT;
        }

        public FormatException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_FORMAT;
        }

        public FormatException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_FORMAT;
        }

        protected FormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}