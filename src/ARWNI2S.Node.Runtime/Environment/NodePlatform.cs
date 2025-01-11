// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

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