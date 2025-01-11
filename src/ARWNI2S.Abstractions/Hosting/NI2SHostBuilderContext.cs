// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine;
using Microsoft.Extensions.Configuration;
using NI2SContext = ARWNI2S.Engine.EngineContext;

namespace ARWNI2S.Hosting
{
    /// <summary>
    /// Context containing the common services on the <see cref="INiisHost" />. Some properties may be null until set by the <see cref="INiisHost" />.
    /// </summary>
    public class NI2SHostBuilderContext
    {
        /// <summary>
        /// The <see cref="INiisHostEnvironment" /> initialized by the <see cref="INiisHost" />.
        /// </summary>
        public INiisHostEnvironment HostingEnvironment { get; set; } = default!;

        /// <summary>
        /// The <see cref="IConfiguration" /> containing the merged configuration of the engine and the <see cref="INiisHost" />.
        /// </summary>
        public IConfiguration Configuration { get; set; } = default!;

        public IEngineContext EngineContext { get; set; } = NI2SContext.Current;

        internal void AddSingleton<T1, T2>()
        {
            throw new NotImplementedException();
        }
    }
}