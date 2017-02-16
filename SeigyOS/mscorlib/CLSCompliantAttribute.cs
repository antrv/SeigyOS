using System.Runtime.InteropServices;

namespace System
{
    [Serializable]
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    [ComVisible(true)]
    // ReSharper disable once InconsistentNaming
    public sealed class CLSCompliantAttribute: Attribute
    {
        private readonly bool _compliant;

        public CLSCompliantAttribute(bool isCompliant)
        {
            _compliant = isCompliant;
        }

        public bool IsCompliant => _compliant;
    }
}