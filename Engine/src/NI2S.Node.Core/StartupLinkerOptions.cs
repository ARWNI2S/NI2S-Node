// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System.Diagnostics.CodeAnalysis;

namespace NI2S.Node
{
    internal static class StartupLinkerOptions
    {
        // We're going to keep all public constructors and public methods on Startup classes
        public const DynamicallyAccessedMemberTypes Accessibility = DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods;
    }
}
