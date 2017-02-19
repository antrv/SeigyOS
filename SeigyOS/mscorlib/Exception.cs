using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Exception))]
    [Serializable]
    [ComVisible(true)]
    public class Exception: ISerializable, _Exception
    {
        private readonly string _message;
        private readonly Exception _innerException;
        private string _helpUrl;
        private string _className;
        private IDictionary _data;
        private int _HResult;
        private MethodBase _exceptionMethod;
        private string _exceptionMethodString;
        private object _stackTrace;
        private string _source;
        [OptionalField]
        private object _watsonBuckets;
        private string _stackTraceString;
        private string _remoteStackTraceString;
        private int _remoteStackIndex;

        public Exception()
        {
        }

        public Exception(string message)
        {
            _message = message;
        }

        public Exception(string message, Exception innerException)
        {
            _message = message;
            _innerException = innerException;
        }

        [SecuritySafeCritical]
        protected Exception(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            Contract.EndContractBlock();

            _className = info.GetString("ClassName");
            _message = info.GetString("Message");
            _data = (IDictionary)(info.GetValueNoThrow("Data", typeof(IDictionary)));
            _innerException = (Exception)info.GetValue("InnerException", typeof(Exception));
            _helpUrl = info.GetString("HelpURL");
            _stackTraceString = info.GetString("StackTraceString");
            _remoteStackTraceString = info.GetString("RemoteStackTraceString");
            _remoteStackIndex = info.GetInt32("RemoteStackIndex");

            _exceptionMethodString = (string)(info.GetValue("ExceptionMethod", typeof(string)));
            HResult = info.GetInt32("HResult");
            _source = info.GetString("Source");

            // Get the WatsonBuckets that were serialized - this is particularly
            // done to support exceptions going across AD transitions.
            // 
            // We use the no throw version since we could be deserializing a pre-V4
            // exception object that may not have this entry. In such a case, we would
            // get null.
            _watsonBuckets = (Object)info.GetValueNoThrow("WatsonBuckets", typeof(byte[]));

            _safeSerializationManager = info.GetValueNoThrow("SafeSerializationManager", typeof(SafeSerializationManager)) as SafeSerializationManager;

            if (_className == null || HResult == 0)
                throw new SerializationException(__Resources.GetResourceString("Serialization_InsufficientState"));

            if (context.State == StreamingContextStates.CrossAppDomain)
            {
                _remoteStackTraceString = _remoteStackTraceString + _stackTraceString;
                _stackTraceString = null;
            }
        }

        public virtual string Message
        {
            get
            {
                if (_message == null)
                {
                    if (_className == null)
                        _className = GetClassName();
                    return __Resources.GetResourceString("Exception_WasThrown", _className);
                }
                return _message;
            }
        }

        public virtual IDictionary Data
        {
            [SecuritySafeCritical] // auto-generated
            get
            {
                if (_data == null)
                    if (IsImmutableAgileException(this))
                        _data = new EmptyReadOnlyDictionaryInternal();
                    else
                        _data = new ListDictionaryInternal();
                return _data;
            }
        }

        public virtual Exception GetBaseException()
        {
            Exception exception = this;
            Exception innerException = _innerException;
            while (innerException != null)
            {
                exception = innerException;
                innerException = innerException._innerException;
            }
            return exception;
        }

        public Exception InnerException => _innerException;

        public MethodBase TargetSite
        {
            [SecuritySafeCritical]
            get
            {
                return GetTargetSiteInternal();
            }
        }

        public virtual string StackTrace
        {
            [SecuritySafeCritical] 
            get
            {
                return GetStackTrace(true);
            }
        }
        public virtual string HelpLink
        {
            get
            {
                return _helpUrl;
            }
            set
            {
                _helpUrl = value;
            }
        }

        public virtual string Source
        {
            [SecurityCritical]
            get
            {
                if (_source == null)
                {
                    StackTrace st = new StackTrace(this, true);
                    if (st.FrameCount > 0)
                    {
                        StackFrame sf = st.GetFrame(0);
                        MethodBase method = sf.GetMethod();

                        Module module = method.Module;

                        RuntimeModule rtModule = module as RuntimeModule;

                        if (rtModule == null)
                        {
                            ModuleBuilder moduleBuilder = module as ModuleBuilder;
                            if (moduleBuilder != null)
                                rtModule = moduleBuilder.InternalModule;
                            else
                                throw new ArgumentException(__Resources.GetResourceString("Argument_MustBeRuntimeReflectionObject"));
                        }

                        _source = rtModule.GetRuntimeAssembly().GetSimpleName();
                    }
                }

                return _source;
            }
            [SecurityCritical]
            set
            {
                _source = value;
            }
        }

        [SecuritySafeCritical] 
        public override string ToString()
        {
            return ToString(true, true);
        }

        protected event EventHandler<SafeSerializationEventArgs> SerializeObjectState
        {
            add { _safeSerializationManager.SerializeObjectState += value; }
            remove { _safeSerializationManager.SerializeObjectState -= value; }
        }

        [SecurityCritical]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            Contract.EndContractBlock();

            string tempStackTraceString = _stackTraceString;

            if (_stackTrace != null)
            {
                if (tempStackTraceString == null)
                {
                    tempStackTraceString = Environment.GetStackTrace(this, true);
                }
                if (_exceptionMethod == null)
                {
                    _exceptionMethod = GetExceptionMethodFromStackTrace();
                }
            }

            if (_source == null)
            {
                _source = Source; // Set the Source information correctly before serialization
            }

            info.AddValue("ClassName", GetClassName(), typeof(string));
            info.AddValue("Message", _message, typeof(string));
            info.AddValue("Data", _data, typeof(IDictionary));
            info.AddValue("InnerException", _innerException, typeof(Exception));
            info.AddValue("HelpURL", _helpUrl, typeof(string));
            info.AddValue("StackTraceString", tempStackTraceString, typeof(string));
            info.AddValue("RemoteStackTraceString", _remoteStackTraceString, typeof(string));
            info.AddValue("RemoteStackIndex", _remoteStackIndex, typeof(int));
            info.AddValue("ExceptionMethod", GetExceptionMethodString(), typeof(string));
            info.AddValue("HResult", HResult);
            info.AddValue("Source", _source, typeof(string));

            // Serialize the Watson bucket details as well
            info.AddValue("WatsonBuckets", _watsonBuckets, typeof(byte[]));

            if (_safeSerializationManager != null && _safeSerializationManager.IsActive)
            {
                info.AddValue("SafeSerializationManager", _safeSerializationManager, typeof(SafeSerializationManager));

                // User classes derived from Exception must have a valid _safeSerializationManager.
                // Exceptions defined in mscorlib don't use this field might not have it initalized (since they are 
                // often created in the VM with AllocateObject instead if the managed construtor)
                // If you are adding code to use a SafeSerializationManager from an mscorlib exception, update
                // this assert to ensure that it fails when that exception's _safeSerializationManager is NULL 
                Contract.Assert(((_safeSerializationManager != null) || (GetType().Assembly == typeof(object).Assembly)), 
                                "User defined exceptions must have a valid _safeSerializationManager");
            
                // Handle serializing any transparent or partial trust subclass data
                _safeSerializationManager.CompleteSerialization(this, info, context);
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            _stackTrace = null;
            if (_safeSerializationManager == null)
            {
                _safeSerializationManager = new SafeSerializationManager();
            }
            else
            {
                _safeSerializationManager.CompleteDeserialization(this);
            }
        }

        public int HResult
        {
            get
            {
                return _HResult;
            }
            protected set
            {
                _HResult = value;
            }
        }
        public new Type GetType()
        {
            return base.GetType();
        }
    }
}