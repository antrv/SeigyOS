namespace SeigyOS.Devices.Disk
{
    public interface IPartitionTable
    {
        bool Valid { get; }

        byte[] BootLoaderCode { get; set; }

        void LoadFrom(IBlockDevice device);

        void SaveTo(IBlockDevice device);
    }
}