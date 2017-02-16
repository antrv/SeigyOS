using System.Runtime.InteropServices;

namespace System
{
    [AttributeUsage(AttributeTargets.Method)]
    [ComVisible(true)]
    // ReSharper disable once InconsistentNaming
    public sealed class MTAThreadAttribute: Attribute
    {
    }
}