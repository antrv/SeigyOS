using System.Runtime.InteropServices;

namespace System
{
    [ComVisible(true)]
    [ContractClass(typeof(IFormattableContract))]
    public interface IFormattable
    {
        [Pure]
        string ToString(string format, IFormatProvider formatProvider);
    }
}