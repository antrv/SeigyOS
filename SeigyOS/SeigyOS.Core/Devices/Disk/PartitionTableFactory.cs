using System;

namespace SeigyOS.Devices.Disk
{
    public struct PartitionTableFactory: IPartitionTableFactory
    {
        public IPartitionTable LoadFrom(IBlockDevice device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            // MBR partition table?
            MbrPartitionTable mbrPartitionTable = new MbrPartitionTable();
            mbrPartitionTable.LoadFrom(device);

            return mbrPartitionTable;
        }
    }
}