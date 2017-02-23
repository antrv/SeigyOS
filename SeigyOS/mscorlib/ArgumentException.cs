using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class ArgumentException: SystemException, ISerializable
    {
        private readonly string _paramName;

        public ArgumentException()
            : base(__Resources.GetResourceString("Arg_ArgumentException"))
        {
            HResult = __HResults.COR_E_ARGUMENT;
        }

        public ArgumentException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_ARGUMENT;
        }

        public ArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_ARGUMENT;
        }

        public ArgumentException(string message, string paramName, Exception innerException)
            : base(message, innerException)
        {
            _paramName = paramName;
            HResult = __HResults.COR_E_ARGUMENT;
        }

        public ArgumentException(string message, string paramName)

            : base(message)
        {
            _paramName = paramName;
            HResult = __HResults.COR_E_ARGUMENT;
        }

        protected ArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _paramName = info.GetString("ParamName");
        }

        public override string Message
        {
            get
            {
                string s = base.Message;
                if (!string.IsNullOrEmpty(_paramName))
                {
                    string resourceString = __Resources.GetResourceString("Arg_ParamName_Name", _paramName);
                    return s + Environment.NewLine + resourceString;
                }
                return s;
            }
        }

        public virtual string ParamName => _paramName;

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            Contract.EndContractBlock();
            base.GetObjectData(info, context);
            info.AddValue("ParamName", _paramName, typeof(string));
        }
    }
}