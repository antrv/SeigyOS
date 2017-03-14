using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class BadImageFormatException: SystemException
    {
        private readonly string _fileName;
        private readonly string _fusionLog;

        public BadImageFormatException()
            : base(__Resources.GetResourceString(__Resources.Arg_BadImageFormatException))
        {
            HResult = __HResults.COR_E_BADIMAGEFORMAT;
        }

        public BadImageFormatException(string message)
            : base(message)
        {
            HResult = __HResults.COR_E_BADIMAGEFORMAT;
        }

        public BadImageFormatException(string message, Exception inner)
            : base(message, inner)
        {
            HResult = __HResults.COR_E_BADIMAGEFORMAT;
        }

        public BadImageFormatException(string message, string fileName)
            : base(message)
        {
            HResult = __HResults.COR_E_BADIMAGEFORMAT;
            _fileName = fileName;
        }

        public BadImageFormatException(string message, string fileName, Exception inner)
            : base(message, inner)
        {
            HResult = __HResults.COR_E_BADIMAGEFORMAT;
            _fileName = fileName;
        }

        public override string Message
        {
            get
            {
                SetMessageField();
                return base.Message;
            }
        }

        private void SetMessageField()
        {
            if (_message == null)
            {
                if ((_fileName == null) &&
                    (HResult == System.__HResults.COR_E_EXCEPTION))
                    _message = Environment.GetResourceString("Arg_BadImageFormatException");

                else
                    _message = FileLoadException.FormatFileLoadExceptionMessage(_fileName, HResult);
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
        }

        public override string ToString()
        {
            string s = GetType().FullName + ": " + Message;

            if (_fileName != null && _fileName.Length != 0)
                s += Environment.NewLine + Environment.GetResourceString("IO.FileName_Name", _fileName);

            if (InnerException != null)
                s = s + " ---> " + InnerException.ToString();

            if (StackTrace != null)
                s += Environment.NewLine + StackTrace;
#if FEATURE_FUSION
            try
            {
                if(FusionLog!=null)
                {
                    if (s==null)
                        s=" ";
                    s+=Environment.NewLine;
                    s+=Environment.NewLine;
                    s+=FusionLog;
                }
            }
            catch(SecurityException)
            {

            }
#endif
            return s;
        }

        protected BadImageFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Base class constructor will check info != null.

            _fileName = info.GetString("BadImageFormat_FileName");
            try
            {
                _fusionLog = info.GetString("BadImageFormat_FusionLog");
            }
            catch
            {
                _fusionLog = null;
            }
        }

        private BadImageFormatException(string fileName, string fusionLog, int hResult)
            : base(null)
        {
            SetErrorCode(hResult);
            _fileName = fileName;
            _fusionLog = fusionLog;
            SetMessageField();
        }

        public string FusionLog
        {
            [SecuritySafeCritical]
            [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
            get
            {
                return _fusionLog;
            }
        }

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("BadImageFormat_FileName", _fileName, typeof(string));
            try
            {
                info.AddValue("BadImageFormat_FusionLog", FusionLog, typeof(string));
            }
            catch (SecurityException)
            {
            }
        }
    }
}