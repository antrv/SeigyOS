using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class ArithmeticException: SystemException
    {
        public ArithmeticException()
            : base(__Resources.GetResourceString(__Resources.Arg_ArithmeticException))
        {
            HResult = __HResults.COR_E_ARITHMETIC;
        }

        public ArithmeticException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_ARITHMETIC;
        }

        public ArithmeticException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_ARITHMETIC;
        }

        protected ArithmeticException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}