using ARWNI2S.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ARWNI2S.Infrastructure.Extensions
{
    internal sealed class DefaultLoggerLevelConfigureOptions : ConfigureOptions<LoggerFilterOptions>
    {
        public DefaultLoggerLevelConfigureOptions(LogLevel level) : base(options => options.MinLevel = level)
        {
        }
    }

    /// <summary>
    /// Extension methods for setting up logging services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class LoggingServiceCollectionExtensions
    {
        /// <summary>
        /// Adds logging services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddLogging(this IServiceCollection services)
        {
            return AddLogging(services, builder => { });
        }

        /// <summary>
        /// Adds logging services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configure">The <see cref="ILoggingBuilder"/> configuration delegate.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddLogging(this IServiceCollection services, Action<ILoggingBuilder> configure)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddOptions();

            services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, TraceLoggerFactory>());
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<LoggerFilterOptions>>(
                new DefaultLoggerLevelConfigureOptions(LogLevel.Information)));

            configure(new TraceLoggingBuilder(services));
            return services;
        }
    }
}
