using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
    [Serializable]
    internal sealed class Empty: ISerializable
    {
        private Empty()
        {
        }

        public static readonly Empty Value = new Empty();

        public override string ToString()
        {
            return string.Empty;
        }

        [SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            Contract.EndContractBlock();
            UnitySerializationHolder.GetUnitySerializationInfo(info, UnitySerializationHolder.EmptyUnity, null, null);
        }
    }
}