namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed class InternalsVisibleToAttribute: Attribute
    {
        private readonly string _assemblyName;
        private bool _allInternalsVisible = true;

        public InternalsVisibleToAttribute(string assemblyName)
        {
            _assemblyName = assemblyName;
        }

        public string AssemblyName => _assemblyName;

        public bool AllInternalsVisible
        {
            get
            {
                return _allInternalsVisible;
            }
            set
            {
                _allInternalsVisible = value;
            }
        }
    }
}