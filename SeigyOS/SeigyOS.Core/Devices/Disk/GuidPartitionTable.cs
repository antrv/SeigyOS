namespace SeigyOS.Devices.Disk
{
    public struct GuidPartitionTable: IPartitionTable
    {
        public bool Valid => throw new System.NotImplementedException();

        public byte[] BootLoaderCode
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        public void LoadFrom(IBlockDevice device)
        {
            throw new System.NotImplementedException();
        }

        public void SaveTo(IBlockDevice device)
        {
            throw new System.NotImplementedException();
        }
    }
}