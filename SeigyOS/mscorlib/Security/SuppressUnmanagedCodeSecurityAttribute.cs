using System.Runtime.InteropServices;

namespace System.Security
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate,
        AllowMultiple = true, Inherited = false)]
    [ComVisible(true)]
    public sealed class SuppressUnmanagedCodeSecurityAttribute: Attribute
    {
    }
}