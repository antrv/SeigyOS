namespace System.Runtime.CompilerServices
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class DependencyAttribute: Attribute
    {
        private readonly string _dependentAssembly;
        private readonly LoadHint _loadHint;

        public DependencyAttribute(string dependentAssemblyArgument, LoadHint loadHintArgument)
        {
            _dependentAssembly = dependentAssemblyArgument;
            _loadHint = loadHintArgument;
        }

        public string DependentAssembly => _dependentAssembly;
        public LoadHint LoadHint => _loadHint;
    }
}