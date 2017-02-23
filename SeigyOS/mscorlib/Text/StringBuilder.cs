using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Text
{
    [ComVisible(true)]
    [Serializable]
    public sealed class StringBuilder: ISerializable
    {
        // TODO: members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}