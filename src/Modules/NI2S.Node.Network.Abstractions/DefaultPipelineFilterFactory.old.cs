using System;

namespace NI2S.Node.Networking
{
    public class DefaultPipelineFilterFactory<TPackageInfo, TPipelineFilter> : PipelineFilterFactoryBase<TPackageInfo>
        where TPipelineFilter : IPipelineFilter<TPackageInfo>, new()
    {
        public DefaultPipelineFilterFactory(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        protected override IPipelineFilter<TPackageInfo> CreateCore(object client)
        {
            return new TPipelineFilter();
        }
    }
}