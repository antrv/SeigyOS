namespace System.Runtime.Versioning
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public sealed class TargetFrameworkAttribute: Attribute
    {
        private readonly string _frameworkName;
        private string _frameworkDisplayName;

        public TargetFrameworkAttribute(string frameworkName)
        {
            if (frameworkName == null)
                throw new ArgumentNullException("frameworkName");
            Contract.EndContractBlock();
            _frameworkName = frameworkName;
        }

        public string FrameworkName => _frameworkName;

        public string FrameworkDisplayName
        {
            get
            {
                return _frameworkDisplayName;
            }
            set
            {
                _frameworkDisplayName = value;
            }
        }
    }
}