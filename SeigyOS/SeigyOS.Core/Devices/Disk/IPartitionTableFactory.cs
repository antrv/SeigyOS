namespace SeigyOS.Devices.Disk
{
    public interface IPartitionTableFactory
    {
        IPartitionTable LoadFrom(IBlockDevice device);
    }
}