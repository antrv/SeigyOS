using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum HostProtectionResource
    {
        None = 0x0,
        Synchronization = 0x1,
        SharedState = 0x2,
        ExternalProcessMgmt = 0x4,
        SelfAffectingProcessMgmt = 0x8,
        ExternalThreading = 0x10,
        SelfAffectingThreading = 0x20,
        SecurityInfrastructure = 0x40,
        // ReSharper disable once InconsistentNaming
        UI = 0x80,
        MayLeakOnAbort = 0x100,
        All = 0x1ff,
    }
}