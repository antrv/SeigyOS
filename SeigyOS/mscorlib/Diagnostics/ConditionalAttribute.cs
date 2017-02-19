using System.Runtime.InteropServices;

namespace System.Diagnostics
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    [ComVisible(true)]
    public sealed class ConditionalAttribute: Attribute
    {
        private readonly string _conditionString;

        public ConditionalAttribute(string conditionString)
        {
            _conditionString = conditionString;
        }

        public string ConditionString => _conditionString;
    }
}