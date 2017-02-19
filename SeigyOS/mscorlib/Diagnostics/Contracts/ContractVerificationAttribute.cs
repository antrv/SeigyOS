namespace System.Diagnostics.Contracts
{
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct |
                    AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property)]
    public sealed class ContractVerificationAttribute: Attribute
    {
        private readonly bool _value;

        public ContractVerificationAttribute(bool value)
        {
            _value = value;
        }

        public bool Value => _value;
    }
}