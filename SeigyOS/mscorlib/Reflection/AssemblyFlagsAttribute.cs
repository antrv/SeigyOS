using System.Runtime.InteropServices;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblyFlagsAttribute: Attribute
    {
        private readonly AssemblyNameFlags _flags;

        [Obsolete("This constructor has been deprecated. Please use AssemblyFlagsAttribute(AssemblyNameFlags) instead.")]
        [CLSCompliant(false)]
        public AssemblyFlagsAttribute(uint flags)
        {
            _flags = (AssemblyNameFlags)flags;
        }

        [Obsolete("This constructor has been deprecated. Please use AssemblyFlagsAttribute(AssemblyNameFlags) instead.")]
        public AssemblyFlagsAttribute(int assemblyFlags)
        {
            _flags = (AssemblyNameFlags)assemblyFlags;
        }

        public AssemblyFlagsAttribute(AssemblyNameFlags assemblyFlags)
        {
            _flags = assemblyFlags;
        }

        [Obsolete("This property has been deprecated. Please use AssemblyFlags instead.")]
        [CLSCompliant(false)]
        public uint Flags => (uint)_flags;

        public int AssemblyFlags => (int)_flags;
    }
}