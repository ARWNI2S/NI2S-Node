using System.Reflection;

namespace ARWNI2S.Infrastructure.Extensions
{
    internal static class AssemblyExtensions
    {
        private static T GetAssemblyAttribute<T>(Assembly assembly) where T : Attribute
        {
            if (assembly == null)
                return null;

            object[] attributes = assembly.GetCustomAttributes(typeof(T), true);

            if (attributes == null || attributes.Length == 0)
                return null;

            return (T)attributes[0];
        }

        internal static string GetAssemblyTitle(this Assembly assembly) { return GetAssemblyAttribute<AssemblyTitleAttribute>(assembly).Title; }
        internal static string GetAssemblyDescription(this Assembly assembly) { return GetAssemblyAttribute<AssemblyDescriptionAttribute>(assembly).Description; }
        internal static string GetAssemblyConfiguration(this Assembly assembly) { return GetAssemblyAttribute<AssemblyConfigurationAttribute>(assembly).Configuration; }
        internal static string GetAssemblyCompany(this Assembly assembly) { return GetAssemblyAttribute<AssemblyCompanyAttribute>(assembly).Company; }
        internal static string GetAssemblyProduct(this Assembly assembly) { return GetAssemblyAttribute<AssemblyProductAttribute>(assembly).Product; }
        internal static string GetAssemblyCopyright(this Assembly assembly) { return GetAssemblyAttribute<AssemblyCopyrightAttribute>(assembly).Copyright; }
        internal static string GetAssemblyTrademark(this Assembly assembly) { return GetAssemblyAttribute<AssemblyTrademarkAttribute>(assembly).Trademark; }
        internal static string GetAssemblyCulture(this Assembly assembly) { return GetAssemblyAttribute<AssemblyCultureAttribute>(assembly).Culture; }
        internal static string GetAssemblyVersion(this Assembly assembly) { return GetAssemblyAttribute<AssemblyVersionAttribute>(assembly).ToString(); }
        internal static string GetAssemblyFileVersion(this Assembly assembly) { return GetAssemblyAttribute<AssemblyFileVersionAttribute>(assembly).Version; }
    }
}
