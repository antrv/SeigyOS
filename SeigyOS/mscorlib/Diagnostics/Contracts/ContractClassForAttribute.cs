namespace System.Diagnostics.Contracts
{
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ContractClassForAttribute: Attribute
    {
        private readonly Type _typeContractFor;

        public ContractClassForAttribute(Type typeContractsAreFor)
        {
            _typeContractFor = typeContractsAreFor;
        }

        public Type TypeContractsAreFor => _typeContractFor;
    }
}