using System;
using System.Runtime.CompilerServices;

namespace SeigyOS.Devices.Disk
{
    public struct MbrPartitionTable: IPartitionTable
    {
        private bool _valid;

        public bool Valid => _valid;

        public byte[] BootLoaderCode { get; set; }

        public void LoadFrom(IBlockDevice device)
        {
            _valid = false;

            if (device == null)
                return;

            const int supportedBlockSize = 512;
            if (device.BlockSize != supportedBlockSize && device.BlockCount < 3)
                return;

            byte* buffer = stackalloc byte[supportedBlockSize];
            device.Read(0, 1, buffer);

            ref byte partRef = ref buffer.Slice(MbrPartitions.Offset).GetPinnableReference();
            ref MbrPartitions partitions = ref Unsafe.As<byte, MbrPartitions>(ref partRef);

            if (partitions.MbrSignature != 0xAA55)
                return;
        }

        public void SaveTo(IBlockDevice device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));
        }
    }
}