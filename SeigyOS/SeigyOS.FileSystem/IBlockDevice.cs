namespace SeigyOS.FileSystem
{
    public interface IBlockDevice
    {
        bool CanWrite { get; }

        ulong BlockCount { get; }

        uint BlockSize { get; }

        void Read(ulong blockIndex, ulong blockCount, IBuffer buffer);

        void Write(ulong blockIndex, ulong blockCount, IReadOnlyBuffer buffer);
    }
}