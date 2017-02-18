using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyDelaySignAttribute: Attribute
    {
        private readonly bool _delaySign;

        public AssemblyDelaySignAttribute(bool delaySign)
        {
            _delaySign = delaySign;
        }

        public bool DelaySign => _delaySign;
    }
}