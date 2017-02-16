namespace System.Runtime.ConstrainedExecution
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor |
                    AttributeTargets.Method | AttributeTargets.Interface, Inherited = false)]
    public sealed class ReliabilityContractAttribute: Attribute
    {
        private readonly Consistency _consistency;
        private readonly Cer _cer;

        public ReliabilityContractAttribute(Consistency consistencyGuarantee, Cer cer)
        {
            _consistency = consistencyGuarantee;
            _cer = cer;
        }

        public Consistency ConsistencyGuarantee => _consistency;
        public Cer Cer => _cer;
    }
}