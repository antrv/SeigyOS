using System.Runtime.InteropServices;

namespace System.Threading
{
    [Serializable]
    [ComVisible(true)]
    public enum ApartmentState
    {
        // ReSharper disable once InconsistentNaming
        STA = 0,
        // ReSharper disable once InconsistentNaming
        MTA = 1,
        Unknown = 2
    }
}