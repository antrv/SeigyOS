using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [Serializable]
    [ComVisible(true)]
    public class SystemException: Exception
    {
        public SystemException()
            : base(__Resources.GetResourceString("Arg_SystemException"))
        {
            HResult = __HResults.COR_E_SYSTEM;
        }

        public SystemException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_SYSTEM;
        }

        public SystemException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_SYSTEM;
        }

        protected SystemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}