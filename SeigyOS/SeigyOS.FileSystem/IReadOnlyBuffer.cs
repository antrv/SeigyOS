namespace SeigyOS.FileSystem
{
    public interface IReadOnlyBuffer
    {
        ulong Size { get; }

        void Read(IBuffer buffer, ulong offset, ulong length);
    }
}