using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum MethodImplOptions
    {
        Unmanaged = MethodImplAttributes.Unmanaged,
        ForwardRef = MethodImplAttributes.ForwardRef,
        PreserveSig = MethodImplAttributes.PreserveSig,
        InternalCall = MethodImplAttributes.InternalCall,
        Synchronized = MethodImplAttributes.Synchronized,
        NoInlining = MethodImplAttributes.NoInlining,

        [ComVisible(false)]
        AggressiveInlining = MethodImplAttributes.AggressiveInlining,
        NoOptimization = MethodImplAttributes.NoOptimization,
    }
}