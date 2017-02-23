namespace System.Runtime.CompilerServices
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class ReferenceAssemblyAttribute: Attribute
    {
        private readonly string _description;

        public ReferenceAssemblyAttribute()
        {
        }

        public ReferenceAssemblyAttribute(string description)
        {
            _description = description;
        }

        public string Description => _description;
    }
}