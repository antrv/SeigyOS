using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class ArrayTypeMismatchException: SystemException
    {
        public ArrayTypeMismatchException()
            : base(__Resources.GetResourceString("Arg_ArrayTypeMismatchException"))
        {
            HResult = __HResults.COR_E_ARRAYTYPEMISMATCH;
        }

        public ArrayTypeMismatchException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_ARRAYTYPEMISMATCH;
        }

        public ArrayTypeMismatchException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_ARRAYTYPEMISMATCH;
        }

        protected ArrayTypeMismatchException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}