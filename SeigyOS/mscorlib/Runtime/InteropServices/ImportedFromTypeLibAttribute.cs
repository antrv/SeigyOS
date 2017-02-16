namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class ImportedFromTypeLibAttribute: Attribute
    {
        private readonly string _val;

        public ImportedFromTypeLibAttribute(string tlbFile)
        {
            _val = tlbFile;
        }

        public string Value => _val;
    }
}