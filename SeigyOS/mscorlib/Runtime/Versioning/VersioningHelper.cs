using System.Security;

namespace System.Runtime.Versioning
{
    public static class VersioningHelper
    {
        public static string MakeVersionSafeName(string name, ResourceScope from, ResourceScope to)
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [ResourceConsumption(ResourceScope.Process, ResourceScope.Process)]
        public static string MakeVersionSafeName(string name, ResourceScope from, ResourceScope to, Type type)
        {
            throw new NotImplementedException();
        }
    }
}