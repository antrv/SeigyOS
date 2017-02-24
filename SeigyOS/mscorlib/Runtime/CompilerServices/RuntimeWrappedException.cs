using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security;

namespace System.Runtime.CompilerServices
{
    [Serializable]
    public sealed class RuntimeWrappedException: Exception
    {
        private readonly object _wrappedException;

        private RuntimeWrappedException(object thrownObject)
            : base(__Resources.GetResourceString("RuntimeWrappedException"))
        {
            HResult = __HResults.COR_E_RUNTIMEWRAPPED;
            _wrappedException = thrownObject;
        }

        public object WrappedException => _wrappedException;

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            Contract.EndContractBlock();
            base.GetObjectData(info, context);
            info.AddValue("WrappedException", _wrappedException, typeof(object));
        }

        internal RuntimeWrappedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _wrappedException = info.GetValue("WrappedException", typeof(object));
        }
    }
}