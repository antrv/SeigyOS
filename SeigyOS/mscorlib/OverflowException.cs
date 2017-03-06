using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class OverflowException: ArithmeticException
    {
        public OverflowException()
            : base(__Resources.GetResourceString(__Resources.Arg_OverflowException))
        {
            HResult = __HResults.COR_E_OVERFLOW;
        }

        public OverflowException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_OVERFLOW;
        }

        public OverflowException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_OVERFLOW;
        }

        protected OverflowException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}