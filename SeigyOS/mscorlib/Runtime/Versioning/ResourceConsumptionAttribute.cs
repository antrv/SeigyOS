namespace System.Runtime.Versioning
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor, Inherited = false)]
    [Conditional("RESOURCE_ANNOTATION_WORK")]
    public sealed class ResourceConsumptionAttribute: Attribute
    {
        private readonly ResourceScope _consumptionScope;
        private readonly ResourceScope _resourceScope;

        public ResourceConsumptionAttribute(ResourceScope resourceScope)
        {
            _resourceScope = resourceScope;
            _consumptionScope = _resourceScope;
        }

        public ResourceConsumptionAttribute(ResourceScope resourceScope, ResourceScope consumptionScope)
        {
            _resourceScope = resourceScope;
            _consumptionScope = consumptionScope;
        }

        public ResourceScope ResourceScope => _resourceScope;
        public ResourceScope ConsumptionScope => _consumptionScope;
    }
}