// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Node.Hosting.Internal
{
    internal static class HostingPathResolver
    {
        public static string ResolvePath(string contentRootPath) =>
            PathWithDirectorySeperatorAtEnd(ResolvePathNonCononical(contentRootPath, AppContext.BaseDirectory));

        public static string ResolvePath(string contentRootPath, string basePath) =>
            PathWithDirectorySeperatorAtEnd(ResolvePathNonCononical(contentRootPath, basePath));

        private static string PathWithDirectorySeperatorAtEnd(string path) =>
            Path.EndsInDirectorySeparator(path) ? path : path + Path.DirectorySeparatorChar;

        private static string ResolvePathNonCononical(string contentRootPath, string basePath)
        {
            if (string.IsNullOrEmpty(contentRootPath))
            {
                return Path.GetFullPath(basePath);
            }
            if (Path.IsPathRooted(contentRootPath))
            {
                return Path.GetFullPath(contentRootPath);
            }
            return Path.GetFullPath(Path.Combine(Path.GetFullPath(basePath), contentRootPath));
        }
    }
}