﻿using System.Text.Encodings.Web;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;

namespace NI2S.Node.Hosting.Extensions
{
    /// <summary>
    /// Options to configure JSON serialization settings for <see cref="HttpRequestJsonExtensions"/>
    /// and <see cref="HttpResponseJsonExtensions"/>.
    /// </summary>
    public class JsonOptions
    {
        internal static readonly JsonSerializerOptions DefaultSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            // Web defaults don't use the relaxed JSON escaping encoder.
            //
            // Because these options are for producing content that is written directly to the request
            // (and not embedded in an HTML page for example), we can use UnsafeRelaxedJsonEscaping.
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,

            // The JsonSerializerOptions.GetTypeInfo method is called directly and needs a defined resolver
            // setting the default resolver (reflection-based) but the user can overwrite it directly or by modifying
            // the TypeInfoResolverChain
            TypeInfoResolver = TrimmingAppContextSwitches.EnsureJsonTrimmability ? null : CreateDefaultTypeResolver()
        };

        // Use a copy so the defaults are not modified.
        /// <summary>
        /// Gets the <see cref="JsonSerializerOptions"/>.
        /// </summary>
        public JsonSerializerOptions SerializerOptions { get; } = new JsonSerializerOptions(DefaultSerializerOptions);

#pragma warning disable IL2026 // Suppressed in Microsoft.AspNetCore.Http.Extensions.WarningSuppressions.xml
#pragma warning disable IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
        private static IJsonTypeInfoResolver CreateDefaultTypeResolver()
            => new DefaultJsonTypeInfoResolver();
#pragma warning restore IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
#pragma warning restore IL2026 // Suppressed in Microsoft.AspNetCore.Http.Extensions.WarningSuppressions.xml
    }
}