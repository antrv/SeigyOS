using System.Runtime.InteropServices;

namespace System.Diagnostics
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Struct,
        Inherited = false)]
    [ComVisible(true)]
    public sealed class DebuggerNonUserCodeAttribute: Attribute
    {
    }
}