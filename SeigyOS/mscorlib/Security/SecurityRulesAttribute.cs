namespace System.Security
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class SecurityRulesAttribute: Attribute
    {
        private readonly SecurityRuleSet _ruleSet;
        private bool _skipVerificationInFullTrust;

        public SecurityRulesAttribute(SecurityRuleSet ruleSet)
        {
            _ruleSet = ruleSet;
        }

        public bool SkipVerificationInFullTrust
        {
            get
            {
                return _skipVerificationInFullTrust;
            }
            set
            {
                _skipVerificationInFullTrust = value;
            }
        }

        public SecurityRuleSet RuleSet => _ruleSet;
    }
}