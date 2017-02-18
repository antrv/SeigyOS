using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
    [ComVisible(true)]
    public class ResolveEventArgs: EventArgs
    {
        private readonly string _name;
        private readonly Assembly _requestingAssembly;

        public ResolveEventArgs(string name)
        {
            _name = name;
        }

        public ResolveEventArgs(string name, Assembly requestingAssembly)
        {
            _name = name;
            _requestingAssembly = requestingAssembly;
        }

        public string Name => _name;
        public Assembly RequestingAssembly => _requestingAssembly;
    }
}