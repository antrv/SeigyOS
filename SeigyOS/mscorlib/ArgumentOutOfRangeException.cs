using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using JetBrains.Annotations;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class ArgumentOutOfRangeException: ArgumentException, ISerializable
    {
        private static volatile string _rangeMessage;
        private readonly object _actualValue;

        private static string RangeMessage
        {
            get
            {
                if (_rangeMessage == null)
                    _rangeMessage = __Resources.GetResourceString("Arg_ArgumentOutOfRangeException");
                return _rangeMessage;
            }
        }

        public ArgumentOutOfRangeException()
            : base(RangeMessage)
        {
            HResult = __HResults.COR_E_ARGUMENTOUTOFRANGE;
        }

        public ArgumentOutOfRangeException([InvokerParameterName] string paramName)
            : base(RangeMessage, paramName)
        {
            HResult = __HResults.COR_E_ARGUMENTOUTOFRANGE;
        }

        public ArgumentOutOfRangeException([InvokerParameterName] string paramName, string message)
            : base(message, paramName)
        {
            HResult = __HResults.COR_E_ARGUMENTOUTOFRANGE;
        }

        public ArgumentOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = __HResults.COR_E_ARGUMENTOUTOFRANGE;
        }

        public ArgumentOutOfRangeException([InvokerParameterName] string paramName, object actualValue, string message)
            : base(message, paramName)
        {
            _actualValue = actualValue;
            HResult = __HResults.COR_E_ARGUMENTOUTOFRANGE;
        }

        public override string Message
        {
            get
            {
                string s = base.Message;
                if (_actualValue != null)
                {
                    string valueMessage = __Resources.GetResourceString("ArgumentOutOfRange_ActualValue", _actualValue.ToString());
                    if (s == null)
                        return valueMessage;
                    return s + Environment.NewLine + valueMessage;
                }
                return s;
            }
        }

        public virtual object ActualValue => _actualValue;

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            Contract.EndContractBlock();
            base.GetObjectData(info, context);
            info.AddValue("ActualValue", _actualValue, typeof(object));
        }

        protected ArgumentOutOfRangeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _actualValue = info.GetValue("ActualValue", typeof(object));
        }
    }
}