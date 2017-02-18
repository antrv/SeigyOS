using System.Runtime.InteropServices;

namespace System.Reflection
{
    [ComVisible(true)]
    public class AssemblyNameProxy: MarshalByRefObject
    {
        [ResourceExposure(ResourceScope.Machine)]
        [ResourceConsumption(ResourceScope.Machine)]
        public AssemblyName GetAssemblyName(string assemblyFile)
        {
            return AssemblyName.GetAssemblyName(assemblyFile);
        }
    }
}