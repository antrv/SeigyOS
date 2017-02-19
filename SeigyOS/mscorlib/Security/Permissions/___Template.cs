﻿using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Assembly |
        AttributeTargets.Delegate, AllowMultiple = true, Inherited = false)]
    [ComVisible(true)]
    [Serializable]
    sealed public class HostProtectionAttribute: CodeAccessSecurityAttribute
    {
        private HostProtectionResource m_resources = HostProtectionResource.None;

        public HostProtectionAttribute()
            : base(SecurityAction.LinkDemand)
        {
        }

        public HostProtectionAttribute(SecurityAction action)
            : base(action)
        {
            if (action != SecurityAction.LinkDemand)
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"));
            Contract.EndContractBlock();
        }

        public HostProtectionResource Resources
        {
            get
            {
                return m_resources;
            }
            set
            {
                m_resources = value;
            }
        }

        public bool Synchronization
        {
            get
            {
                return (m_resources & HostProtectionResource.Synchronization) != 0;
            }
            set
            {
                m_resources = (value ? m_resources | HostProtectionResource.Synchronization : m_resources & ~HostProtectionResource.Synchronization);
            }
        }

        public bool SharedState
        {
            get
            {
                return (m_resources & HostProtectionResource.SharedState) != 0;
            }
            set
            {
                m_resources = (value ? m_resources | HostProtectionResource.SharedState : m_resources & ~HostProtectionResource.SharedState);
            }
        }

        public bool ExternalProcessMgmt
        {
            get
            {
                return (m_resources & HostProtectionResource.ExternalProcessMgmt) != 0;
            }
            set
            {
                m_resources = (value ? m_resources | HostProtectionResource.ExternalProcessMgmt : m_resources & ~HostProtectionResource.ExternalProcessMgmt);
            }
        }

        public bool SelfAffectingProcessMgmt
        {
            get
            {
                return (m_resources & HostProtectionResource.SelfAffectingProcessMgmt) != 0;
            }
            set
            {
                m_resources = (value
                    ? m_resources | HostProtectionResource.SelfAffectingProcessMgmt : m_resources & ~HostProtectionResource.SelfAffectingProcessMgmt);
            }
        }

        public bool ExternalThreading
        {
            get
            {
                return (m_resources & HostProtectionResource.ExternalThreading) != 0;
            }
            set
            {
                m_resources = (value ? m_resources | HostProtectionResource.ExternalThreading : m_resources & ~HostProtectionResource.ExternalThreading);
            }
        }

        public bool SelfAffectingThreading
        {
            get
            {
                return (m_resources & HostProtectionResource.SelfAffectingThreading) != 0;
            }
            set
            {
                m_resources = (value
                    ? m_resources | HostProtectionResource.SelfAffectingThreading : m_resources & ~HostProtectionResource.SelfAffectingThreading);
            }
        }

        [System.Runtime.InteropServices.ComVisible(true)]
        public bool SecurityInfrastructure
        {
            get
            {
                return (m_resources & HostProtectionResource.SecurityInfrastructure) != 0;
            }
            set
            {
                m_resources = (value
                    ? m_resources | HostProtectionResource.SecurityInfrastructure : m_resources & ~HostProtectionResource.SecurityInfrastructure);
            }
        }

        public bool UI
        {
            get
            {
                return (m_resources & HostProtectionResource.UI) != 0;
            }
            set
            {
                m_resources = (value ? m_resources | HostProtectionResource.UI : m_resources & ~HostProtectionResource.UI);
            }
        }

        public bool MayLeakOnAbort
        {
            get
            {
                return (m_resources & HostProtectionResource.MayLeakOnAbort) != 0;
            }
            set
            {
                m_resources = (value ? m_resources | HostProtectionResource.MayLeakOnAbort : m_resources & ~HostProtectionResource.MayLeakOnAbort);
            }
        }

        public override IPermission CreatePermission()
        {
            if (_unrestricted)
                return new HostProtectionPermission(PermissionState.Unrestricted);
            return new HostProtectionPermission(m_resources);
        }
    }
}