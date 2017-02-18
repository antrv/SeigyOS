using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyAlgorithmIdAttribute: Attribute
    {
        private readonly uint _algId;

        public AssemblyAlgorithmIdAttribute(AssemblyHashAlgorithm algorithmId)
        {
            _algId = (uint)algorithmId;
        }

        [CLSCompliant(false)]
        public AssemblyAlgorithmIdAttribute(uint algorithmId)
        {
            _algId = algorithmId;
        }

        [CLSCompliant(false)]
        public uint AlgorithmId => _algId;
    }
}