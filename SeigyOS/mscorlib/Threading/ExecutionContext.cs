using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Security;

namespace System.Threading
{
    [Serializable]
    public sealed class ExecutionContext: IDisposable, ISerializable
    {
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal ExecutionContext()
        {
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal ExecutionContext(bool isPreAllocatedDefault)
        {
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        [SecurityCritical]
        public static void Run(ExecutionContext executionContext, ContextCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        public ExecutionContext CreateCopy()
        {
            throw new NotImplementedException();
        }

        [SecurityCritical]
        public static AsyncFlowControl SuppressFlow()
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        public static void RestoreFlow()
        {
            throw new NotImplementedException();
        }

        [Pure]
        public static bool IsFlowSuppressed()
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ExecutionContext Capture()
        {
            throw new NotImplementedException();
        }

        [SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }
    }
}