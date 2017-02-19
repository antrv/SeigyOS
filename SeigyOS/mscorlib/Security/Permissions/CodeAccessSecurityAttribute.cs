using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Assembly,
        AllowMultiple = true, Inherited = false)]
    [ComVisible(true)]
    public abstract class CodeAccessSecurityAttribute: SecurityAttribute
    {
        protected CodeAccessSecurityAttribute(SecurityAction action)
            : base(action)
        {
        }
    }
}