using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Method)]
    [ComVisible(true)]
    public class CompilationRelaxationsAttribute: Attribute
    {
        private readonly int _relaxations;

        public CompilationRelaxationsAttribute(int relaxations)
        {
            _relaxations = relaxations;
        }

        public CompilationRelaxationsAttribute(CompilationRelaxations relaxations)
        {
            _relaxations = (int)relaxations;
        }

        public int CompilationRelaxations => _relaxations;
    }
}