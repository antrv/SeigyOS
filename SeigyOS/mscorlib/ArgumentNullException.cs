using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using JetBrains.Annotations;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class ArgumentNullException: ArgumentException
    {
        public ArgumentNullException()
            : base(__Resources.GetResourceString(__Resources.ArgumentNull_Generic))
        {
            HResult = __HResults.E_POINTER;
        }

        public ArgumentNullException([InvokerParameterName] string paramName)
            : base(__Resources.GetResourceString(__Resources.ArgumentNull_Generic), paramName)
        {
            HResult = __HResults.E_POINTER;
        }

        public ArgumentNullException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.E_POINTER;
        }

        public ArgumentNullException([InvokerParameterName] string paramName, string message)
            : base(message, paramName)
        {
            HResult = __HResults.E_POINTER;
        }

        [SecurityCritical]
        protected ArgumentNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}