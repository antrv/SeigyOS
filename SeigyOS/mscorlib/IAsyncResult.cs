using System.Runtime.InteropServices;

namespace System
{
    [ComVisible(true)]
    public interface IAsyncResult
    {
        bool IsCompleted { get; }
        WaitHandle AsyncWaitHandle { get; }
        object AsyncState { get; }
        bool CompletedSynchronously { get; }
    }
}