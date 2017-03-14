using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class OutOfMemoryException: SystemException
    {
        public OutOfMemoryException()
            : base(GetMessageFromNativeResources(ExceptionMessageKind.OutOfMemory))
        {
            HResult = __HResults.COR_E_OUTOFMEMORY;
        }

        public OutOfMemoryException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_OUTOFMEMORY;
        }

        public OutOfMemoryException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_OUTOFMEMORY;
        }

        protected OutOfMemoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}