using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
    [ComVisible(true)]
    public class AssemblyLoadEventArgs: EventArgs
    {
        private readonly Assembly _loadedAssembly;

        public AssemblyLoadEventArgs(Assembly loadedAssembly)
        {
            _loadedAssembly = loadedAssembly;
        }

        public Assembly LoadedAssembly => _loadedAssembly;
    }
}