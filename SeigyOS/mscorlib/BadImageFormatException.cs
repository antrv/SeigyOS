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
                if (_fileName == null && HResult == __HResults.COR_E_EXCEPTION)
                    _message = Environment.GetResourceString("Arg_BadImageFormatException");

                else
                    _message = FileLoadException.FormatFileLoadExceptionMessage(_fileName, HResult);
            }
        }

        public string FileName => _fileName;

        public override string ToString()
        {
            string s = GetType().FullName + ": " + Message;

            if (!string.IsNullOrEmpty(_fileName))
                s += Environment.NewLine + __Resources.GetResourceString(__Resources.IO_FileName_Name, _fileName);

            if (InnerException != null)
                s = s + " ---> " + InnerException;

            if (StackTrace != null)
                s += Environment.NewLine + StackTrace;

            try
            {
                if (FusionLog != null)
                {
                    s += Environment.NewLine;
                    s += Environment.NewLine;
                    s += FusionLog;
                }
            }
            catch (SecurityException)
            {

            }
            return s;
        }

        protected BadImageFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
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
            HResult = hResult;
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