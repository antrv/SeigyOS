using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public sealed class IndexOutOfRangeException: SystemException
    {
        public IndexOutOfRangeException()
            : base(__Resources.GetResourceString(__Resources.Arg_IndexOutOfRangeException))
        {
            HResult = __HResults.COR_E_INDEXOUTOFRANGE;
        }

        public IndexOutOfRangeException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_INDEXOUTOFRANGE;
        }

        public IndexOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_INDEXOUTOFRANGE;
        }

        internal IndexOutOfRangeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}