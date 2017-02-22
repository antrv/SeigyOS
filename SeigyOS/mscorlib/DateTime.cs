using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [StructLayout(LayoutKind.Auto)]
    [Serializable]
    public struct DateTime: IComparable, IFormattable, IConvertible, ISerializable, IComparable<DateTime>, IEquatable<DateTime>
    {
        // TODO: members
    }
}