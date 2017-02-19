namespace System.Runtime.Versioning
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct |
                    AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Method |
                    AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Event, 
        AllowMultiple = false, Inherited = false)]
    public sealed class ComponentGuaranteesAttribute: Attribute
    {
        private readonly ComponentGuaranteesOptions _guarantees;

        public ComponentGuaranteesAttribute(ComponentGuaranteesOptions guarantees)
        {
            _guarantees = guarantees;
        }

        public ComponentGuaranteesOptions Guarantees => _guarantees;
    }
}