using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
    [ComVisible(true)]
    [Serializable]
    public class SerializationException: SystemException
    {
        public SerializationException()
            : base(__Resources.GetResourceString(__Resources.Arg_SerializationException))
        {
            HResult = __HResults.COR_E_SERIALIZATION;
        }

        public SerializationException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_SERIALIZATION;
        }

        public SerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_SERIALIZATION;
        }

        protected SerializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}