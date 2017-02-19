using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    [ComVisible(true)]
    public sealed class DebuggerBrowsableAttribute: Attribute
    {
        private readonly DebuggerBrowsableState _state;

        public DebuggerBrowsableAttribute(DebuggerBrowsableState state)
        {
            if (state < DebuggerBrowsableState.Never || state > DebuggerBrowsableState.RootHidden)
                throw new ArgumentOutOfRangeException(nameof(state));
            Contract.EndContractBlock();
            _state = state;
        }

        public DebuggerBrowsableState State => _state;
    }
}