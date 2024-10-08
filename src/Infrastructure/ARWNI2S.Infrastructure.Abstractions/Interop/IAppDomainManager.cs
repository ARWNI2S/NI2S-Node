using System.Runtime.InteropServices;

namespace ARWNI2S.Infrastructure.Interop
{
    [Guid("E18F6C9C-CB32-4057-96AB-708B47467338"), ComVisible(true)]
    public interface IAppDomainManager
    {
        void Run(string assemblyFilename, string friendlyName);
    }
}
