namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    [ComVisible(true)]
    public sealed class BestFitMappingAttribute: Attribute
    {
        private readonly bool _bestFitMapping;

        public BestFitMappingAttribute(bool bestFitMapping)
        {
            _bestFitMapping = bestFitMapping;
        }

        public bool BestFitMapping => _bestFitMapping;
        public bool ThrowOnUnmappableChar;
    }
}