using System.Diagnostics.Contracts;

namespace System.__Helpers
{
    internal struct GuidResult
    {
        internal Guid parsedGuid;
        internal GuidParseThrowStyle throwStyle;
        internal ParseFailureKind _failure;
        internal string m_failureMessageID;
        internal object m_failureMessageFormatArgument;
        internal string m_failureArgumentName;
        internal Exception m_innerException;

        internal void Init(GuidParseThrowStyle canThrow)
        {
            parsedGuid = Guid.Empty;
            throwStyle = canThrow;
        }

        internal void SetFailure(Exception nativeException)
        {
            _failure = ParseFailureKind.NativeException;
            m_innerException = nativeException;
        }

        internal void SetFailure(ParseFailureKind failure, string failureMessageId, object failureMessageFormatArgument = null,
            string failureArgumentName = null, Exception innerException = null)
        {
            Contract.Assert(failure != ParseFailureKind.NativeException, "ParseFailureKind.NativeException should not be used with this overload");
            _failure = failure;
            m_failureMessageID = failureMessageId;
            m_failureMessageFormatArgument = failureMessageFormatArgument;
            m_failureArgumentName = failureArgumentName;
            m_innerException = innerException;
            if (throwStyle != GuidParseThrowStyle.None)
                throw GetGuidParseException();
        }

        internal Exception GetGuidParseException()
        {
            switch (_failure)
            {
                case ParseFailureKind.ArgumentNull:
                    return new ArgumentNullException(m_failureArgumentName, __Resources.GetResourceString(m_failureMessageID));
                case ParseFailureKind.FormatWithInnerException:
                    return new FormatException(__Resources.GetResourceString(m_failureMessageID), m_innerException);
                case ParseFailureKind.FormatWithParameter:
                    return new FormatException(__Resources.GetResourceString(m_failureMessageID, m_failureMessageFormatArgument));
                case ParseFailureKind.Format:
                    return new FormatException(__Resources.GetResourceString(m_failureMessageID));
                case ParseFailureKind.NativeException:
                    return m_innerException;
                default:
                    Contract.Assert(false, "Unknown GuidParseFailure: " + _failure);
                    return new FormatException(__Resources.GetResourceString(__Resources.Format_GuidUnrecognized));
            }
        }
    }
}