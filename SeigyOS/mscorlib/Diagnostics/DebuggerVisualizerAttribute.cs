using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    [ComVisible(true)]
    public sealed class DebuggerVisualizerAttribute: Attribute
    {
        private readonly string _visualizerObjectSourceName;
        private readonly string _visualizerName;
        private string _description;
        private string _targetName;
        private Type _target;

        public DebuggerVisualizerAttribute(string visualizerTypeName)
        {
            _visualizerName = visualizerTypeName;
        }

        public DebuggerVisualizerAttribute(string visualizerTypeName, string visualizerObjectSourceTypeName)
        {
            _visualizerName = visualizerTypeName;
            _visualizerObjectSourceName = visualizerObjectSourceTypeName;
        }

        public DebuggerVisualizerAttribute(string visualizerTypeName, Type visualizerObjectSource)
        {
            if (visualizerObjectSource == null)
                throw new ArgumentNullException(nameof(visualizerObjectSource));
            Contract.EndContractBlock();
            _visualizerName = visualizerTypeName;
            _visualizerObjectSourceName = visualizerObjectSource.AssemblyQualifiedName;
        }

        public DebuggerVisualizerAttribute(Type visualizer)
        {
            if (visualizer == null)
                throw new ArgumentNullException(nameof(visualizer));
            Contract.EndContractBlock();
            _visualizerName = visualizer.AssemblyQualifiedName;
        }

        public DebuggerVisualizerAttribute(Type visualizer, Type visualizerObjectSource)
        {
            if (visualizer == null)
                throw new ArgumentNullException(nameof(visualizer));
            if (visualizerObjectSource == null)
                throw new ArgumentNullException(nameof(visualizerObjectSource));
            Contract.EndContractBlock();
            _visualizerName = visualizer.AssemblyQualifiedName;
            _visualizerObjectSourceName = visualizerObjectSource.AssemblyQualifiedName;
        }

        public DebuggerVisualizerAttribute(Type visualizer, string visualizerObjectSourceTypeName)
        {
            if (visualizer == null)
                throw new ArgumentNullException(nameof(visualizer));
            Contract.EndContractBlock();
            _visualizerName = visualizer.AssemblyQualifiedName;
            _visualizerObjectSourceName = visualizerObjectSourceTypeName;
        }

        public string VisualizerObjectSourceTypeName => _visualizerObjectSourceName;
        public string VisualizerTypeName => _visualizerName;

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public Type Target
        {
            get
            {
                return _target;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                Contract.EndContractBlock();
                _targetName = value.AssemblyQualifiedName;
                _target = value;
            }
        }

        public string TargetTypeName
        {
            set
            {
                _targetName = value;
            }
            get
            {
                return _targetName;
            }
        }
    }
}