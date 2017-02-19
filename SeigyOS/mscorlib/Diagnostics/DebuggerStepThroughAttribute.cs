using System.Runtime.InteropServices;

namespace System.Diagnostics
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Constructor, Inherited = false)]
    [ComVisible(true)]
    public sealed class DebuggerStepThroughAttribute: Attribute
    {
    }
}