using System;

namespace SeigyOS
{
    public interface IBuffer: IReadOnlyBuffer
    {
        void CopyFrom(IntPtr source, IntPtr count, IntPtr bufferOffset);

        void CopyFrom(byte[] source, int sourceIndex, IntPtr bufferOffset);
    }
}