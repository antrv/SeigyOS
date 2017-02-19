using System.Security;

namespace System.Runtime.Versioning
{
    public static class CompatibilitySwitch
    {
        [SecurityCritical]
        public static bool IsEnabled(string compatibilitySwitchName)
        {
            throw new NotImplementedException();
        }

        [SecurityCritical]
        public static string GetValue(string compatibilitySwitchName)
        {
            throw new NotImplementedException();
        }
    }
}