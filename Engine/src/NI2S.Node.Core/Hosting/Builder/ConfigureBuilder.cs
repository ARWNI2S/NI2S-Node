// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Reflection;

namespace NI2S.Node.Hosting.Builder
{
    internal sealed class ConfigureBuilder
    {
        public ConfigureBuilder(MethodInfo configure)
        {
            MethodInfo = configure;
        }

        public MethodInfo MethodInfo { get; }

        public Action<INodeEngineBuilder> Build(object instance) => builder => Invoke(instance, builder);

        private void Invoke(object instance, INodeEngineBuilder builder)
        {
            // Create a scope for Configure, this allows creating scoped dependencies
            // without the hassle of manually creating a scope.
            using var scope = builder.EngineServices.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var parameterInfos = MethodInfo.GetParameters();
            var parameters = new object[parameterInfos.Length];
            for (var index = 0; index < parameterInfos.Length; index++)
            {
                var parameterInfo = parameterInfos[index];
                if (parameterInfo.ParameterType == typeof(INodeEngineBuilder))
                {
                    parameters[index] = builder;
                }
                else
                {
                    try
                    {
                        parameters[index] = serviceProvider.GetRequiredService(parameterInfo.ParameterType);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format(
                            CultureInfo.InvariantCulture,
                            "Could not resolve a service of type '{0}' for the parameter '{1}' of method '{2}' on type '{3}'.",
                            parameterInfo.ParameterType.FullName,
                            parameterInfo.Name,
                            MethodInfo.Name,
                            MethodInfo.DeclaringType?.FullName), ex);
                    }
                }
            }

            MethodInfo.InvokeWithoutWrappingExceptions(instance, parameters);
        }
    }
}