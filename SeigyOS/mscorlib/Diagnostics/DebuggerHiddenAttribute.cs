using System.Runtime.InteropServices;

namespace System.Diagnostics
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor, Inherited = false)]
    [ComVisible(true)]
    public sealed class DebuggerHiddenAttribute: Attribute
    {
    }
}