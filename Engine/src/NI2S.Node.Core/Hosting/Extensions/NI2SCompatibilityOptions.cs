// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using System;

namespace NI2S.Node.Hosting.Extensions
{
    /// <summary>
    /// An options type for configuring the engine <see cref="Mvc.CompatibilityVersion"/>.
    /// </summary>
    /// <remarks>
    /// The primary way to configure the engine's <see cref="Mvc.CompatibilityVersion"/> is by
    /// calling <see cref="MvcCoreMvcBuilderExtensions.SetCompatibilityVersion(IMvcBuilder, CompatibilityVersion)"/>
    /// or <see cref="MvcCoreMvcCoreBuilderExtensions.SetCompatibilityVersion(IMvcCoreBuilder, CompatibilityVersion)"/>.
    /// </remarks>
    [Obsolete("This API is obsolete and will be removed in a future version. Consider removing usages.",
        DiagnosticId = "ASP5001",
        UrlFormat = "https://aka.ms/aspnetcore-warnings/{0}")]
    public class NI2SCompatibilityOptions
    {
        /// <summary>
        /// Gets or sets the engine's configured <see cref="Mvc.CompatibilityVersion"/>.
        /// </summary>
        /// <value>the default value is <see cref="CompatibilityVersion.Version_3_0"/>.</value>
        public CompatibilityVersion CompatibilityVersion { get; set; } = CompatibilityVersion.Version_3_0;
    }
}