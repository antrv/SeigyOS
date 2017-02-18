using System.Runtime.InteropServices;

namespace System.Security
{
    [ComVisible(true)]
    public interface IEvidenceFactory
    {
        Evidence Evidence { get; }
    }
}