using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
    [ComVisible(true)]
    // ReSharper disable once InconsistentNaming
    public sealed class IUnknownConstantAttribute: CustomConstantAttribute
    {
        public override object Value => new UnknownWrapper(null);
    }
}