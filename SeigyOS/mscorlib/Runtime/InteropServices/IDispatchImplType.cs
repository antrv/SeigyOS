namespace System.Runtime.InteropServices
{
    [Obsolete("The IDispatchImplAttribute is deprecated.", false)]
    [Serializable]
    [ComVisible(true)]
    // ReSharper disable once InconsistentNaming
    public enum IDispatchImplType
    {
        SystemDefinedImpl = 0,
        InternalImpl = 1,
        CompatibleImpl = 2,
    }
}