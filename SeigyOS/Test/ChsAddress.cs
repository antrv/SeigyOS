using System.Runtime.InteropServices;

namespace SeigyOS.Devices.Disk
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ChsAddress
    {
        internal byte Head;
        internal byte CylHighSector;
        internal byte CylLow;

        internal ushort Cylinder
        {
            get => (ushort)(((CylHighSector & 0xC0u) << 2) | CylLow);
            set
            {
                CylHighSector = (byte)(((value >> 2) & 0xC0u) | (CylHighSector & 0x3Fu));
                CylLow = (byte)value;
            }
        }

        internal byte Sector
        {
            get => (byte)(CylHighSector & 0x3Fu);
            set => CylHighSector = (byte)((CylHighSector & 0xC0u) | (value & 0x3Fu));
        }
    }
}