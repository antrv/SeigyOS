using System.Runtime.InteropServices;

namespace SeigyOS.Devices.Disk
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct MbrPartitions
    {
        internal const int Offset = 0x1B8;

        internal uint DiskSignature;
        internal ushort DiskProtectionSignature;
        internal MbrPartition Partition1;
        internal MbrPartition Partition2;
        internal MbrPartition Partition3;
        internal MbrPartition Partition4;
        internal ushort MbrSignature;
    }
}