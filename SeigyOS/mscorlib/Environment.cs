using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace System
{
    [ComVisible(true)]
    public static class Environment
    {
        // TODO: members

        public static string NewLine
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return "\r\n";
            }
        }
    }
}