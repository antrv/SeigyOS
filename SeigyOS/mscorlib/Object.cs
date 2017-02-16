using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
    [Serializable]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class Object
    {
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public Object()
        {

        }

        public virtual string ToString()
        {
            return GetType().ToString();
        }

        public virtual bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public static bool Equals(object left, object right)
        {
            if (left == right)
            {
                return true;
            }
            if (left == null || right == null)
            {
                return false;
            }
            return left.Equals(right);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static bool ReferenceEquals(object left, object right)
        {
            return left == right;
        }

        public virtual int GetHashCode()
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        [Pure]
        [ResourceExposure(ResourceScope.None)]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public extern Type GetType();

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        ~Object()
        {
        }

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        protected extern object MemberwiseClone();
    }
}