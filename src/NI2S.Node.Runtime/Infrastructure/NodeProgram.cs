using NI2S.Node.Configuration;
using NI2S.Node.Globalization.Resources;
using NI2S.Node.Hosting.Builder;
using NI2S.Node.Infrastructure;
using NI2S.Node.Infrastructure.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NI2S.Node
{
    public abstract class NodeProgram : IDisposable
    {
        #region Argument program options

        private ProgramOptions _options;
        protected ProgramOptions ProgramOptions => _options;

        protected internal void InitializeOptions(string[] args)
        {
            _options = new ProgramOptions(args);
        }

        protected abstract void ParseArguments(string[] args);

        public abstract void PromptHelp(TextWriter logger);

        #endregion

        #region Instance abstractions

        private NI2SNode niisNode;
        protected bool Initialize()
        {
            //TODO: LOCALIZATION
            if (_options == null) throw new InvalidOperationException($"Invalid {nameof(_options)}");

            //TODO USING
            //var builder = NI2SNode.CreateBuilder();
            var builder = NI2SNode.CreateBuilder(_options.Args); 
            //var builder = NI2SNode.CreateBuilder(_options.ExtraArguments);
            try
            {
                //TODO: CONFIGURE FIXED BUILDER CONFIGURATION
                //builder.Configuration.AddJsonFile(NopConfigurationDefaults.AppSettingsFilePath, true, true);
                //if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
                //{
                //    var path = string.Format(NopConfigurationDefaults.AppSettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
                //    builder.Configuration.AddJsonFile(path, true, true);
                //}
                //builder.Configuration.AddEnvironmentVariables();

                //TODO: load application settings
                //builder.Services.ConfigureApplicationSettings(builder);

                //var appSettings = Singleton<AppSettings>.Instance;
                //var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;

                //if (useAutofac)
                //    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
                //else
                //    builder.Host.UseDefaultServiceProvider(options =>
                //    {
                //        //we don't validate the scopes, since at the app start and the initial configuration we need 
                //        //to resolve some services (registered as "scoped") through the root container
                //        options.ValidateScopes = false;
                //        options.ValidateOnBuild = true;
                //    });

                ////add services to the application and configure service provider
                //builder.Services.ConfigureApplicationServices(builder);
                builder = ConfigureBuilder(builder);

                niisNode = builder.Build();
                //var app = builder.Build();

                ////configure the application HTTP request pipeline
                //app.ConfigureRequestPipeline();

            }
            catch
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Convenience configure builder for user code. 
        /// </summary>
        /// <param name="builder">the builder.</param>
        /// <returns>the same <paramref name="builder"/> parameter.</returns>
        protected virtual NI2SNodeBuilder ConfigureBuilder(NI2SNodeBuilder builder)
        {
            return builder;
        }

        protected async Task<int> RunNodeAsync()
        {
            //TODO: LOCALIZATION
            if (_options == null) throw new InvalidOperationException($"Invalid {nameof(_options)}");
            if (niisNode == null) throw new InvalidOperationException($"Invalid {nameof(niisNode)}");

            await niisNode.StartEngineAsync();

            await niisNode.RunAsync();

            return 0;
        }

        #endregion

        #region Program logging
        //protected void LogOperationInfo(OperationInfo operationInfo, TextWriter logger)
        //{

        //}

        protected static void LogMessage(string message, TextWriter logger)
        {
            logger ??= Console.Out;
            if (message != null)
                logger.WriteLine(message);
        }

        protected static void LogWarning(string message, TextWriter logger)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            LogMessage(message, logger);
            Console.ResetColor();
        }

        protected static int ExitWithError(string message, TextWriter logger)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            LogMessage(message, logger);
            Console.ResetColor();
            return 1;
        }

        #endregion

        #region IDisposable Implementation

        private bool _disposed;
        protected bool IsDisposed => _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: eliminar el estado administrado (objetos administrados)
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                _disposed = true;
            }
        }

        // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~NodeProgram()
        // {
        //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    /// <summary>
    /// Encapsulates entry point interactions.
    /// </summary>
    public abstract class NodeProgram<TProgram> : NodeProgram
        where TProgram : NodeProgram<TProgram>, new()
    {
        #region Entry Point

        /// <summary>
        /// Static fixed entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static int ExecuteProgram(string[] args, TextWriter logger = null)
        {
            using var program = new TProgram();
            try
            {
                if (!program.TryParseArguments(args, out var argsOpInfo) ||
                    argsOpInfo.Result == OperationResult.Failure ||
                    argsOpInfo.Result == OperationResult.Errors)
                {
                    return ExitWithError(argsOpInfo.Message, logger);
                }
                else if (argsOpInfo.Result == OperationResult.Warnings)
                    LogWarning(argsOpInfo.Message, logger);

                if (program.ProgramOptions.ShowHelp)
                {
                    program.PromptHelp(logger);
                    return 1;
                }
                if (program.ProgramOptions.ExtraArguments.Count > 1)
                {
                    program.PromptHelp(logger);
                    return ExitWithError(LocalizedStrings.ProgramErrors_ExtraArgumentsCount, logger);
                }

                if (!program.TryInitialize(out var initOpInfo))
                {
                    return ExitWithError(initOpInfo.Message, logger);
                }

                return program.RunNodeAsync().Result;

            }
            catch (Exception ex)
            {
                return ExitWithError(ex.Message, logger);
            }
        }

        #endregion

        #region Constructor

        protected NodeProgram() : base() { }

        #endregion

        #region Program Options

        private bool TryParseArguments(string[] args, out OperationInfo operationInfo)
        {
            try
            {
                InitializeOptions(args);
                ParseArguments(args);
                operationInfo = OperationInfo.Success;
            }
            catch (NotImplementedException niEx)
            {
                //TODO: Exception caching.
                operationInfo = OperationInfo.SuccessWithState(niEx);
                return true;
            }
            catch (Exception)
            {
                //TODO: Exception caching.
                operationInfo = OperationInfo.Failure;
            }
            return operationInfo.Result == OperationResult.Success;
        }

        /// <summary>
        /// Do not call.
        /// </summary>
        /// <exception cref="NotImplementedException">throws if called.</exception>
        protected override void ParseArguments(string[] args)
        {
            throw new NotImplementedException(LocalizedStrings.NotImplementedException_Override_Message);
        }

        #endregion

        #region Initialization

        private bool _isInitialized;

        private bool TryInitialize(out OperationInfo operationInfo)
        {
            try
            {
                if (_isInitialized)
                    throw new InvalidOperationException("Already initialized");

                _isInitialized = Initialize();

                operationInfo = _isInitialized ? OperationInfo.Success : OperationInfo.Failure;
            }
            catch (Exception ex)
            {
                operationInfo = _isInitialized ? OperationInfo.SuccessWithState(ex.Message) : OperationInfo.FailWithException(ex.Message, ex);
            }
            return _isInitialized;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    // TODO: eliminar el estado administrado (objetos administrados)
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                base.Dispose(disposing);
            }
        }
    }
}
