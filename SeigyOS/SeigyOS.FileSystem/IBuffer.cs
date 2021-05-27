namespace SeigyOS.FileSystem
{
    public interface IBuffer: IReadOnlyBuffer
    {
        void Write(ReadOnlyBuffer buffer, ulong offset, ulong length);
    }

    public ref struct ReadOnlyBuffer
    {
        private readonly Pinnable _pinnable;
        private readonly ulong _size;

        public ReadOnlyBuffer(byte[] data)
        {
            _pinnable = new Pinnable() { Ref = data };
            _size = (ulong)data.Length;
        }
    }

    internal sealed class Pinnable
    {
        internal object Ref;
    }
}