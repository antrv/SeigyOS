using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class AppDomainUnloadedException: SystemException
    {
        public AppDomainUnloadedException()
            : base(__Resources.GetResourceString(__Resources.Arg_AppDomainUnloadedException))
        {
            HResult = __HResults.COR_E_APPDOMAINUNLOADED;
        }

        public AppDomainUnloadedException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_APPDOMAINUNLOADED;
        }

        public AppDomainUnloadedException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_APPDOMAINUNLOADED;
        }

        protected AppDomainUnloadedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}