namespace System.Runtime.InteropServices
{
    // ReSharper disable once InconsistentNaming
    [Obsolete("The IDispatchImplAttribute is deprecated.", false)]
    [Serializable]
    [ComVisible(true)]
    public enum IDispatchImplType
    {
        SystemDefinedImpl = 0,
        InternalImpl = 1,
        CompatibleImpl = 2,
    }
}