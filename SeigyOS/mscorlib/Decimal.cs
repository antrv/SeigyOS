using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    [ComVisible(true)]
    public struct Decimal: IFormattable, IComparable, IConvertible, IDeserializationCallback, IComparable<Decimal>, IEquatable<Decimal>
    {
        // TODO: members
    }
}