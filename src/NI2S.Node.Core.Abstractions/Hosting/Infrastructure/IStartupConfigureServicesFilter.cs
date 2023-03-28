using Microsoft.Extensions.DependencyInjection;
using System;

namespace NI2S.Node.Hosting.Infrastructure
{
    /// <summary>
    /// This API supports the ASP.NET Core infrastructure and is not intended to be used
    /// directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [Obsolete]
    public interface IStartupConfigureServicesFilter
    {
        /// <summary>
        /// Extends the provided <paramref name="next"/> and returns a modified <see cref="Action"/> action of the same type.
        /// </summary>
        /// <param name="next">The ConfigureServices method to extend.</param>
        /// <returns>A modified <see cref="Action"/>.</returns>
        Action<IServiceCollection> ConfigureServices(Action<IServiceCollection> next);
    }
}
