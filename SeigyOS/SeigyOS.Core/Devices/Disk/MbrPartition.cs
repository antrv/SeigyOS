using System.Runtime.InteropServices;

namespace SeigyOS.Devices.Disk
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MbrPartition
    {
        internal byte Status;
        internal ChsAddress StartChs;
        internal byte PartitionType;
        internal ChsAddress EndChs;
        internal uint StartLba;
        internal uint SectorCount;
    }
}