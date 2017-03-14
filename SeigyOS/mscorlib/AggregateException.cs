using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
    [Serializable]
    [DebuggerDisplay("Count = {" + nameof(InnerExceptionCount) + "}")]
    public class AggregateException : Exception
    {
        private readonly ReadOnlyCollection<Exception> _innerExceptions;

        public AggregateException()
            : base(__Resources.GetResourceString(__Resources.AggregateException_ctor_DefaultMessage))
        {
            _innerExceptions = new ReadOnlyCollection<Exception>(Array.Empty<Exception>());
        }

        public AggregateException(string message)
            : base(message)
        {
            _innerExceptions = new ReadOnlyCollection<Exception>(Array.Empty<Exception>());
        }

        public AggregateException(string message, Exception innerException)
            : base(message, innerException)
        {
            if (innerException == null)
                throw new ArgumentNullException(nameof(innerException));
            _innerExceptions = new ReadOnlyCollection<Exception>(new Exception[] { innerException });
        }

        public AggregateException(IEnumerable<Exception> innerExceptions) :
            this(__Resources.GetResourceString(__Resources.AggregateException_ctor_DefaultMessage), innerExceptions)
        {
        }

        public AggregateException(params Exception[] innerExceptions) :
            this(__Resources.GetResourceString(__Resources.AggregateException_ctor_DefaultMessage), innerExceptions)
        {
        }

        public AggregateException(string message, IEnumerable<Exception> innerExceptions)
            : this(message, innerExceptions as IList<Exception> ?? (innerExceptions == null ? (List<Exception>)null : new List<Exception>(innerExceptions)))
        {
        }

        public AggregateException(string message, params Exception[] innerExceptions) :
            this(message, (IList<Exception>)innerExceptions)
        {
        }

        private AggregateException(string message, IList<Exception> innerExceptions)
            : base(message, innerExceptions != null && innerExceptions.Count > 0 ? innerExceptions[0] : null)
        {
            if (innerExceptions == null)
                throw new ArgumentNullException(nameof(innerExceptions));

            Exception[] exceptionsCopy = new Exception[innerExceptions.Count];
            for (int i = 0; i < exceptionsCopy.Length; i++)
            {
                exceptionsCopy[i] = innerExceptions[i];
                if (exceptionsCopy[i] == null)
                    throw new ArgumentException(__Resources.GetResourceString(__Resources.AggregateException_ctor_InnerExceptionNull));
            }
            _innerExceptions = new ReadOnlyCollection<Exception>(exceptionsCopy);
        }

        internal AggregateException(IEnumerable<ExceptionDispatchInfo> innerExceptionInfos) :
            this(__Resources.GetResourceString(__Resources.AggregateException_ctor_DefaultMessage), innerExceptionInfos)
        {
        }

        internal AggregateException(string message, IEnumerable<ExceptionDispatchInfo> innerExceptionInfos)
            : this(message, innerExceptionInfos as IList<ExceptionDispatchInfo> ??
                            (innerExceptionInfos == null ?
                                (List<ExceptionDispatchInfo>)null :
                                new List<ExceptionDispatchInfo>(innerExceptionInfos)))
        {
        }

        private AggregateException(string message, IList<ExceptionDispatchInfo> innerExceptionInfos)
            : base(message, innerExceptionInfos != null && innerExceptionInfos.Count > 0 && innerExceptionInfos[0] != null ?
                innerExceptionInfos[0].SourceException : null)
        {
            if (innerExceptionInfos == null)
                throw new ArgumentNullException(nameof(innerExceptionInfos));

            Exception[] exceptionsCopy = new Exception[innerExceptionInfos.Count];
            for (int i = 0; i < exceptionsCopy.Length; i++)
            {
                var edi = innerExceptionInfos[i];
                if (edi != null) exceptionsCopy[i] = edi.SourceException;
                if (exceptionsCopy[i] == null)
                    throw new ArgumentException(__Resources.GetResourceString(__Resources.AggregateException_ctor_InnerExceptionNull));
            }
            _innerExceptions = new ReadOnlyCollection<Exception>(exceptionsCopy);
        }

        [SecurityCritical]
        protected AggregateException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            Exception[] innerExceptions = info.GetValue("InnerExceptions", typeof(Exception[])) as Exception[];
            if (innerExceptions == null)
                throw new SerializationException(__Resources.GetResourceString(__Resources.AggregateException_DeserializationFailure));
            _innerExceptions = new ReadOnlyCollection<Exception>(innerExceptions);
        }

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            base.GetObjectData(info, context);
            Exception[] innerExceptions = new Exception[_innerExceptions.Count];
            _innerExceptions.CopyTo(innerExceptions, 0);
            info.AddValue("InnerExceptions", innerExceptions, typeof(Exception[]));
        }

        public override Exception GetBaseException()
        {
            Exception back = this;
            AggregateException backAsAggregate = this;
            while (backAsAggregate != null && backAsAggregate.InnerExceptions.Count == 1)
            {
                back = back.InnerException;
                backAsAggregate = back as AggregateException;
            }
            return back;
        }

        public ReadOnlyCollection<Exception> InnerExceptions => _innerExceptions;

        public void Handle(Func<Exception, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            List<Exception> unhandledExceptions = null;
            for (int i = 0; i < _innerExceptions.Count; i++)
            {
                if (!predicate(_innerExceptions[i]))
                {
                    if (unhandledExceptions == null)
                        unhandledExceptions = new List<Exception>();
                    unhandledExceptions.Add(_innerExceptions[i]);
                }
            }
            if (unhandledExceptions != null)
                throw new AggregateException(Message, unhandledExceptions);
        }

        public AggregateException Flatten()
        {
            List<Exception> flattenedExceptions = new List<Exception>();
            List<AggregateException> exceptionsToFlatten = new List<AggregateException>();
            exceptionsToFlatten.Add(this);
            int nDequeueIndex = 0;
            while (exceptionsToFlatten.Count > nDequeueIndex)
            {
                IList<Exception> currentInnerExceptions = exceptionsToFlatten[nDequeueIndex++].InnerExceptions;
                for (int i = 0; i < currentInnerExceptions.Count; i++)
                {
                    Exception currentInnerException = currentInnerExceptions[i];
                    if (currentInnerException == null)
                        continue;
                    AggregateException currentInnerAsAggregate = currentInnerException as AggregateException;
                    if (currentInnerAsAggregate != null)
                        exceptionsToFlatten.Add(currentInnerAsAggregate);
                    else
                        flattenedExceptions.Add(currentInnerException);
                }
            }
            return new AggregateException(Message, flattenedExceptions);
        }

        public override string ToString()
        {
            string text = base.ToString();
            for (int i = 0; i < _innerExceptions.Count; i++)
            {
                text = string.Format(CultureInfo.InvariantCulture, __Resources.GetResourceString(__Resources.AggregateException_ToString),
                    text, Environment.NewLine, i, _innerExceptions[i].ToString(), "<---", Environment.NewLine);
            }

            return text;
        }

        private int InnerExceptionCount => InnerExceptions.Count;
    }
}