using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Assembly,
        AllowMultiple = true, Inherited = false)]
    [ComVisible(true)]
    [Serializable]
    public sealed class EnvironmentPermissionAttribute: CodeAccessSecurityAttribute
    {
        private string _read;
        private string _write;

        public EnvironmentPermissionAttribute(SecurityAction action)
            : base(action)
        {
        }

        public string Read
        {
            get
            {
                return _read;
            }
            set
            {
                _read = value;
            }
        }

        public string Write
        {
            get
            {
                return _write;
            }
            set
            {
                _write = value;
            }
        }

        public string All
        {
            get
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_GetMethod"));
            }
            set
            {
                _write = value;
                _read = value;
            }
        }

        public override IPermission CreatePermission()
        {
            if (_unrestricted)
                return new EnvironmentPermission(PermissionState.Unrestricted);

            EnvironmentPermission perm = new EnvironmentPermission(PermissionState.None);
            if (_read != null)
                perm.SetPathList(EnvironmentPermissionAccess.Read, _read);
            if (_write != null)
                perm.SetPathList(EnvironmentPermissionAccess.Write, _write);
            return perm;
        }
    }
}