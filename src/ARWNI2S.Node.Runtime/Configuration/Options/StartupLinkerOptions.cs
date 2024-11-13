using System.Diagnostics.CodeAnalysis;

namespace ARWNI2S.Node.Configuration.Options
{
    internal static class StartupLinkerOptions
    {
        // We're going to keep all public constructors and public methods on Startup classes
        public const DynamicallyAccessedMemberTypes Accessibility = DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods;
    }
}