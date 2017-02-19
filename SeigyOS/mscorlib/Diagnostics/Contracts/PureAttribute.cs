namespace System.Diagnostics.Contracts
{
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property |
                    AttributeTargets.Event | AttributeTargets.Delegate | AttributeTargets.Class |
                    AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class PureAttribute: Attribute
    {
    }
}