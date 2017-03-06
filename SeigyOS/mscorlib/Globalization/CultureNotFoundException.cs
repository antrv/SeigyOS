using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System.Globalization
{
    [ComVisible(true)]
    [Serializable]
    public class CultureNotFoundException: ArgumentException, ISerializable
    {
        private readonly string _invalidCultureName;
        private readonly int? _invalidCultureId;

        public CultureNotFoundException()
            : base(__Resources.GetResourceString(__Resources.Argument_CultureNotSupported))
        {
        }

        public CultureNotFoundException(string message)
            : base(message)
        {
        }

        public CultureNotFoundException(string paramName, string message)
            : base(message, paramName)
        {
        }

        public CultureNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public CultureNotFoundException(string paramName, int invalidCultureId, string message)
            : base(message, paramName)
        {
            _invalidCultureId = invalidCultureId;
        }

        public CultureNotFoundException(string message, int invalidCultureId, Exception innerException)
            : base(message, innerException)
        {
            _invalidCultureId = invalidCultureId;
        }

        public CultureNotFoundException(string paramName, string invalidCultureName, string message)
            : base(message, paramName)
        {
            _invalidCultureName = invalidCultureName;
        }

        public CultureNotFoundException(string message, string invalidCultureName, Exception innerException)
            : base(message, innerException)
        {
            _invalidCultureName = invalidCultureName;
        }

        protected CultureNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _invalidCultureId = (int?)info.GetValue("InvalidCultureId", typeof(int?));
            _invalidCultureName = (string)info.GetValue("InvalidCultureName", typeof(string));
        }

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            Contract.EndContractBlock();
            base.GetObjectData(info, context);
            int? invalidCultureId = null;
            invalidCultureId = _invalidCultureId;
            info.AddValue("InvalidCultureId", invalidCultureId, typeof(int?));
            info.AddValue("InvalidCultureName", _invalidCultureName, typeof(string));
        }

        public virtual int? InvalidCultureId => _invalidCultureId;
        public virtual string InvalidCultureName => _invalidCultureName;

        private string FormatedInvalidCultureId
        {
            get
            {
                if (InvalidCultureId != null)
                    return string.Format(CultureInfo.InvariantCulture, "{0} (0x{0:x4})", (int)InvalidCultureId);
                return InvalidCultureName;
            }
        }

        public override string Message
        {
            get
            {
                string s = base.Message;
                if (_invalidCultureId != null || _invalidCultureName != null)
                {
                    string valueMessage = __Resources.GetResourceString(__Resources.Argument_CultureInvalidIdentifier, FormatedInvalidCultureId);
                    if (s == null)
                        return valueMessage;
                    return s + Environment.NewLine + valueMessage;
                }
                return s;
            }
        }
    }
}