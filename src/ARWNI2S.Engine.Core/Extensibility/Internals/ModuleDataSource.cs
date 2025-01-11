using ARWNI2S.Extensibility;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace ARWNI2S.Engine.Extensibility.Internals
{
    /// <summary>
    /// Provides a collection of <see cref="IModule"/> instances.
    /// </summary>
    public abstract class ModuleDataSource : IModuleDataSource
    {
        /// <summary>
        /// Gets a <see cref="IChangeToken"/> used to signal invalidation of cached <see cref="IModule"/>
        /// instances.
        /// </summary>
        /// <returns>The <see cref="IChangeToken"/>.</returns>
        public abstract IChangeToken GetChangeToken();

        /// <summary>
        /// Returns a read-only collection of <see cref="IModule"/> instances.
        /// </summary>
        public abstract IReadOnlyList<IModule> Modules { get; }
        public object RoutePatternFactory { get; private set; }

        ///// <summary>
        ///// Get the <see cref="IModule"/> instances for this <see cref="ModuleDataSource"/> given the specified <see cref="ModuleGroupContext.Processor"/> and <see cref="ModuleGroupContext.Dependencies"/>.
        ///// </summary>
        ///// <param name="context">Details about how the returned <see cref="IModule"/> instances should be grouped and a reference to application services.</param>
        ///// <returns>
        ///// Returns a read-only collection of <see cref="IModule"/> instances given the specified group <see cref="ModuleGroupContext.Processor"/> and <see cref="ModuleGroupContext.Dependencies"/>.
        ///// </returns>
        public virtual IReadOnlyList<IModule> GetGroupedModules(ModuleGroupContext context)
        {
            // Only evaluate Modules once per call.
            var modules = Modules;
            var wrappedModules = new IModule[modules.Count];

            for (int i = 0; i < modules.Count; i++)
            {
                var module = modules[i];

                // IModule does not provide a RoutePattern but ModuleBase does. So it's impossible to apply a prefix for custom Modules.
                // Supporting arbitrary Modules just to add group metadata would require changing the IModule type breaking any real scenario.
                if (module is not ModuleBase moduleBase)
                {
                    throw new NotSupportedException(/*Resources.FormatMapGroup_CustomModuleUnsupported(module.GetType())*/);
                }

                wrappedModules[i] = moduleBase;


                //// Make the full route pattern visible to IModuleConventionBuilder extension methods called on the group.
                //// This includes patterns from any parent groups.
                //var moduleProcessor = ModuleProcessorFactory.Create(moduleBase, context);
                //var moduleBuilder = new ModuleBuilder(moduleBase, moduleProcessor)
                //{
                //    DisplayName = moduleBase.DisplayName,
                //    EngineServices = context.EngineServices,
                //};

                //// Apply group conventions to each module in the group at a lower precedent than metadata already on the module.
                //foreach (var convention in context.Conventions)
                //{
                //    convention(moduleBuilder);
                //}

                //// Any metadata already on the ModuleBase must have been applied directly to the module or to a nested group.
                //// This makes the metadata more specific than what's being applied to this group. So add it after this group's conventions.
                //foreach (var metadata in moduleBase.Metadata)
                //{
                //    moduleBuilder.Metadata.Add(metadata);
                //}

                //foreach (var finallyConvention in context.FinallyConventions)
                //{
                //    finallyConvention(moduleBuilder);
                //}

                //// The RoutePattern, RequestDelegate, Order and DisplayName can all be overridden by non-group-aware conventions.
                //// Unlike with metadata, if a convention is applied to a group that changes any of these, I would expect these
                //// to be overridden as there's no reasonable way to merge these properties.
                //wrappedModules[i] = (ModuleBase)moduleBuilder.Build();
            }

            return wrappedModules;
        }

        // We don't implement DebuggerDisplay directly on the ModuleDataSource base type because this could have side effects.
        internal static string GetDebuggerDisplayStringForModules(IReadOnlyList<IModule> modules)
        {
            if (modules is null || modules.Count == 0)
            {
                return "No modules";
            }

            var sb = new StringBuilder();

            foreach (var module in modules)
            {
                if (module is EngineModule engineModule)
                {
                    var name = engineModule.SystemName;
                    name = string.IsNullOrEmpty(name) ? engineModule.GetType().Name.ToModuleSystemName() : name;
                    sb.Append(name);
                    //sb.Append(", Defaults: new { ");
                    //FormatValues(sb, engineModule.RoutePattern.Defaults);
                    //sb.Append(" }");
                    //var routeNameMetadata = engineModule.Metadata.GetMetadata<IRouteNameMetadata>();
                    //sb.Append(", Route Name: ");
                    //sb.Append(routeNameMetadata?.RouteName);

                    var dependencies = engineModule.ModuleDependencies;

                    if (dependencies.Count > 0)
                    {
                        sb.Append(", Required Dependencies: new { ");
                        FormatDependencies(sb, dependencies);
                        sb.Append(" }");
                    }

                    sb.Append(", Order: ");
                    sb.Append(engineModule.Order);

                    //var httpMethodMetadata = engineModule.Metadata.GetMetadata<IHttpMethodMetadata>();

                    //if (httpMethodMetadata is not null)
                    //{
                    //    sb.Append(", Http Methods: ");
                    //    sb.AppendJoin(", ", httpMethodMetadata.HttpMethods);
                    //}

                    sb.Append(", Display Name: ");
                }
                else
                {
                    sb.Append("Non-EngineModule. DisplayName: ");
                }

                sb.AppendLine(module.DisplayName);
            }

            return sb.ToString();

            static void FormatDependencies(StringBuilder sb, IList<string> dependencies)
            {
                var isFirst = true;

                foreach (string value in dependencies)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        sb.Append(", ");
                    }

                    sb.Append(value);
                }
            }
            //static void FormatValues(StringBuilder sb, IEnumerable<KeyValuePair<string, object>> values)
            //{
            //    var isFirst = true;

            //    foreach (var (key, value) in values)
            //    {
            //        if (isFirst)
            //        {
            //            isFirst = false;
            //        }
            //        else
            //        {
            //            sb.Append(", ");
            //        }

            //        sb.Append(key);
            //        sb.Append(" = ");

            //        if (value is null)
            //        {
            //            sb.Append("null");
            //        }
            //        else
            //        {
            //            sb.Append('\"');
            //            sb.Append(value);
            //            sb.Append('\"');
            //        }
            //    }
            //}
        }
    }
}