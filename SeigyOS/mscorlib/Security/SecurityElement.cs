using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security
{
    [Serializable]
    [ComVisible(true)]
    public sealed class SecurityElement
    {
        // TODO: members
    }

    [Serializable]
    [StrongNameIdentityPermission(SecurityAction.InheritanceDemand, Name = "mscorlib", PublicKey = "0x" + AssemblyRef.EcmaPublicKeyFull)]
    [ComVisible(true)]
    public class PermissionSet: ISecurityEncodable, ICollection, IStackWalk, IDeserializationCallback
    {
    }
}