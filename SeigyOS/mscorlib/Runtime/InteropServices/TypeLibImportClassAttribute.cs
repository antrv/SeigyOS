namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Interface, Inherited = false)]
    [ComVisible(true)]
    public sealed class TypeLibImportClassAttribute: Attribute
    {
        private readonly string _importClassName;

        public TypeLibImportClassAttribute(Type importClass)
        {
            _importClassName = importClass.ToString();
        }

        public string Value => _importClassName;
    }
}