using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    [Serializable]
    [ComVisible(true)]
    public enum MethodCodeType
    {
        // ReSharper disable once InconsistentNaming
        IL = MethodImplAttributes.IL,
        Native = MethodImplAttributes.Native,

        // ReSharper disable once InconsistentNaming
        OPTIL = MethodImplAttributes.OPTIL,
        Runtime = MethodImplAttributes.Runtime
    }
}