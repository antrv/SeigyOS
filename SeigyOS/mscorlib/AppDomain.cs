using System.Runtime.InteropServices;
using System.Security;

namespace System
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_AppDomain))]
    [ComVisible(true)]
    public sealed class AppDomain: MarshalByRefObject, _AppDomain, IEvidenceFactory
    {
        // TODO: members
    }
}