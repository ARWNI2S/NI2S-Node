using System.Runtime.InteropServices;

namespace ARWNI2S.Environment
{
    public interface IPlatform
    {
        OSPlatform OSPlatform { get; }
    }
}