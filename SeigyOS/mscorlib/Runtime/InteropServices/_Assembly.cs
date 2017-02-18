using System.Reflection;
using System.Security;

namespace System.Runtime.InteropServices
{
    [Guid("17156360-2f1a-384a-bc52-fde93c215c5b")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [TypeLibImportClass(typeof(Assembly))]
    [CLSCompliant(false)]
    [ComVisible(true)]
    // ReSharper disable once InconsistentNaming
    public interface _Assembly
    {
        string ToString();
        bool Equals(object other);
        int GetHashCode();
        Type GetType();

        string CodeBase
        {
            [SecurityCritical]
            get;
        }

        string EscapedCodeBase { get; }

        [SecurityCritical]
        AssemblyName GetName();

        [SecurityCritical]
        AssemblyName GetName(bool copiedName);

        string FullName { get; }
        MethodInfo EntryPoint { get; }
        Type GetType(string name);
        Type GetType(string name, bool throwOnError);
        Type[] GetExportedTypes();
        Type[] GetTypes();
        Stream GetManifestResourceStream(Type type, string name);
        Stream GetManifestResourceStream(string name);

        [SecurityCritical]
        FileStream GetFile(string name);

        FileStream[] GetFiles();

        [SecurityCritical]
        FileStream[] GetFiles(bool getResourceModules);

        string[] GetManifestResourceNames();
        ManifestResourceInfo GetManifestResourceInfo(string resourceName);

        string Location
        {
            [SecurityCritical]
            get;
        }

        Evidence Evidence { get; }
        object[] GetCustomAttributes(Type attributeType, bool inherit);
        object[] GetCustomAttributes(bool inherit);
        bool IsDefined(Type attributeType, bool inherit);

        [SecurityCritical]
        void GetObjectData(SerializationInfo info, StreamingContext context);

        [method: SecurityCritical]
        event ModuleResolveEventHandler ModuleResolve;

        Type GetType(string name, bool throwOnError, bool ignoreCase);
        Assembly GetSatelliteAssembly(CultureInfo culture);
        Assembly GetSatelliteAssembly(CultureInfo culture, Version version);
        Module LoadModule(string moduleName, byte[] rawModule);
        Module LoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore);
        object CreateInstance(string typeName);
        object CreateInstance(string typeName, bool ignoreCase);

        object CreateInstance(string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture,
            object[] activationAttributes);

        Module[] GetLoadedModules();
        Module[] GetLoadedModules(bool getResourceModules);
        Module[] GetModules();
        Module[] GetModules(bool getResourceModules);
        Module GetModule(string name);
        AssemblyName[] GetReferencedAssemblies();
        bool GlobalAssemblyCache { get; }
    }
}