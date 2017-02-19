namespace System.Runtime.Versioning
{
    [Flags]
    [Serializable]
    public enum ComponentGuaranteesOptions
    {
        None = 0,
        Exchange = 0x1,
        Stable = 0x2,
        SideBySide = 0x4,
    }
}