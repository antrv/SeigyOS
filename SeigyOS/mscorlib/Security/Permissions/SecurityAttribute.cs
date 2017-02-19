using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Class |
                    AttributeTargets.Struct | AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    [ComVisible(true)]
    public abstract class SecurityAttribute: Attribute
    {
        private SecurityAction _action;
        private bool _unrestricted;

        protected SecurityAttribute(SecurityAction action)
        {
            _action = action;
        }

        public SecurityAction Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value;
            }
        }

        public bool Unrestricted
        {
            get
            {
                return _unrestricted;
            }
            set
            {
                _unrestricted = value;
            }
        }

        public abstract IPermission CreatePermission();
    }
}