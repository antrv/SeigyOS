using System.Security;

namespace System.Threading
{
    public struct AsyncFlowControl: IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        public void Undo()
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public bool Equals(AsyncFlowControl obj)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(AsyncFlowControl a, AsyncFlowControl b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(AsyncFlowControl a, AsyncFlowControl b)
        {
            return !(a == b);
        }
    }
}