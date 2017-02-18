using System.Runtime.InteropServices;

namespace System.Resources
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    [ComVisible(true)]
    public sealed class NeutralResourcesLanguageAttribute: Attribute
    {
        private readonly string _culture;
        private readonly UltimateResourceFallbackLocation _fallbackLoc;

        public NeutralResourcesLanguageAttribute(string cultureName)
        {
            if (cultureName == null)
                throw new ArgumentNullException("cultureName");
            Contract.EndContractBlock();

            _culture = cultureName;
            _fallbackLoc = UltimateResourceFallbackLocation.MainAssembly;
        }

        public NeutralResourcesLanguageAttribute(string cultureName, UltimateResourceFallbackLocation location)
        {
            if (cultureName == null)
                throw new ArgumentNullException("cultureName");
            if (!Enum.IsDefined(typeof(UltimateResourceFallbackLocation), location))
                throw new ArgumentException(Environment.GetResourceString("Arg_InvalidNeutralResourcesLanguage_FallbackLoc", location));
            Contract.EndContractBlock();

            _culture = cultureName;
            _fallbackLoc = location;
        }

        public string CultureName => _culture;
        public UltimateResourceFallbackLocation Location => _fallbackLoc;
    }
}