using System.Runtime.InteropServices;

namespace System
{
    [Serializable]
    [ComVisible(true)]
    public abstract class ValueType
    {
        [SecuritySafeCritical]
        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public override extern int GetHashCode();

        public override string ToString()
        {
            return GetType().ToString();
        }
    }
}