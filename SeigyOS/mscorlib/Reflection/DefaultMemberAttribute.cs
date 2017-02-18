using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    [ComVisible(true)]
    public sealed class DefaultMemberAttribute: Attribute
    {
        private readonly string _memberName;

        public DefaultMemberAttribute(string memberName)
        {
            _memberName = memberName;
        }

        public string MemberName => _memberName;
    }
}