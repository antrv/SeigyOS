namespace System.Runtime.CompilerServices
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class DefaultDependencyAttribute: Attribute
    {
        private readonly LoadHint _loadHint;

        public DefaultDependencyAttribute(LoadHint loadHintArgument)
        {
            _loadHint = loadHintArgument;
        }

        public LoadHint LoadHint => _loadHint;
    }
}