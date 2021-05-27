using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SeigyOS;
using SeigyOS.Devices.Disk;

namespace Test
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct Mbr
    {
        internal fixed byte Code[0x1B8];
        internal MbrPartitions Partitions;
    }

    internal static class Program
    {
        private static void Main()
        {
            //NativeSize s = 1;
            //s += 10;

            //IntPtr ptr = (IntPtr)25;
            //ptr += s;

            //int comparison = s.CompareTo(ptr);

            //Console.WriteLine(comparison);


            byte[] data = new byte[512];

            ref Mbr mbr = ref Unsafe.As<byte, Mbr>(ref data[0]);
            mbr.Partitions.DiskSignature = 0x12345678;
            mbr.Partitions.DiskProtectionSignature = 0x5A5A;
            mbr.Partitions.MbrSignature = 0xAA55;

            ref MbrPartition partition1 = ref mbr.Partitions.Partition1;
            partition1.Status = 0x80;
            partition1.PartitionType = 0xEE;
            partition1.StartLba = 0x10;
            partition1.SectorCount = 10000;

            Enumerable.Range(0, 512 / 16).Select(line => string.Join(" ", Enumerable.Range(0, 16).Select(col => data[line * 16 + col].ToString("X2")))).
                ToList().ForEach(Console.WriteLine);

            Console.ReadLine();
        }
    }
}