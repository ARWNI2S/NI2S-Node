using ARWNI2S.Environment;
using System.Runtime.InteropServices;

namespace ARWNI2S.Node.Environment
{
    public static class NodePlatform
    {
        public static IPlatform PlatformNull { get; } = PlatformImpl.Empty;

        private class PlatformImpl : IPlatform
        {
            internal static readonly PlatformImpl Empty = new();

            protected PlatformImpl()
            {

            }

            public OSPlatform OSPlatform => throw new NotImplementedException();
        }

    }
}