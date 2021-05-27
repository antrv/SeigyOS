using System;

namespace SeigyOS
{
    public interface IReadOnlyBuffer
    {
        IntPtr Size { get; }

        void CopyTo(IntPtr bufferOffset, IntPtr count, IntPtr destination);

        void CopyTo(IntPtr bufferOffset, IntPtr count, byte[] destination, int destinationIndex);
    }
}