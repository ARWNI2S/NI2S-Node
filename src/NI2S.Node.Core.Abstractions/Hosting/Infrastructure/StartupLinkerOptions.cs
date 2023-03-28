using System.Diagnostics.CodeAnalysis;

namespace NI2S.Node
{
    //TODO: FIND CORRECT ASSEMBLY TO MOVE TO
    internal static class StartupLinkerOptions
    {
        // We're going to keep all public constructors and public methods on Startup classes
        public const DynamicallyAccessedMemberTypes Accessibility = DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods;
    }
}
