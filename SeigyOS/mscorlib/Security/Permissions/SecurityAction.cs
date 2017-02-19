using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
    [Serializable]
    [ComVisible(true)]
    public enum SecurityAction
    {
        Demand = 2,
        Assert = 3,

        [Obsolete("Deny is obsolete and will be removed in a future release of the .NET Framework. " +
                  "See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
        Deny = 4,

        PermitOnly = 5,
        LinkDemand = 6,
        InheritanceDemand = 7,

        [Obsolete("Assembly level declarative security is obsolete and is no longer enforced by the CLR by default. " +
                  "See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
        RequestMinimum = 8,

        [Obsolete("Assembly level declarative security is obsolete and is no longer enforced by the CLR by default. " +
                  "See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
        RequestOptional = 9,

        [Obsolete("Assembly level declarative security is obsolete and is no longer enforced by the CLR by default. " +
                  "See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
        RequestRefuse = 10,
    }
}