using System.Runtime.InteropServices;

namespace System.Security
{
    [ComVisible(true)]
    public interface ISecurityPolicyEncodable
    {
        SecurityElement ToXml(PolicyLevel level);
        void FromXml(SecurityElement e, PolicyLevel level);
    }
}