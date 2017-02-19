using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class NotImplementedException: SystemException
    {
        // TODO: members
    }

    [ComVisible(true)]
    [Serializable]
    public class NotSupportedException: SystemException
    {
        public NotSupportedException()
            : base(Environment.GetResourceString("Arg_NotSupportedException"))
        {
            SetErrorCode(__HResults.COR_E_NOTSUPPORTED);
        }

        public NotSupportedException(string message)
            : base(message)
        {
            SetErrorCode(__HResults.COR_E_NOTSUPPORTED);
        }

        public NotSupportedException(string message, Exception innerException)
            : base(message, innerException)
        {
            SetErrorCode(__HResults.COR_E_NOTSUPPORTED);
        }

        protected NotSupportedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

}