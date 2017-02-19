using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
    [Serializable]
    [ComVisible(true)]
    public enum OpCodeType
    {
        [Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
        Annotation = 0,
        Macro = 1,
        Nternal = 2,
        Objmodel = 3,
        Prefix = 4,
        Primitive = 5,
    }
}