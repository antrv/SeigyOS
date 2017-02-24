namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field |
                    AttributeTargets.Event | AttributeTargets.Struct)]
    public sealed class SpecialNameAttribute: Attribute
    {
    }
}