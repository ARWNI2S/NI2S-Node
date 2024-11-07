using System.Buffers;

namespace ARWNI2S.Infrastructure.Network.Protocol.ProxyProtocol
{
    public class ProxyProtocolPipelineFilter<TPackageInfo> : PackagePartsPipelineFilter<TPackageInfo>, IProxyProtocolPipelineFilter
    {
        private readonly IPipelineFilter<TPackageInfo> _applicationPipelineFilter;

        private object _originalFilterContext;

        public ProxyInfo ProxyInfo { get; private set; }

        public ProxyProtocolPipelineFilter(IPipelineFilter<TPackageInfo> applicationPipelineFilter)
        {
            _applicationPipelineFilter = applicationPipelineFilter;
        }

        protected override TPackageInfo CreatePackage()
        {
            return default;
        }

        protected override IPackagePartReader<TPackageInfo> GetFirstPartReader()
        {
            return ProxyProtocolPackagePartReader<TPackageInfo>.ProxyProtocolSwitch;
        }

        public override void Reset()
        {
            // This method will be called when the proxy package handling finishes
            ProxyInfo.Prepare();
            NextFilter = _applicationPipelineFilter;
            base.Reset();
            Context = _originalFilterContext;
        }

        public override TPackageInfo Filter(ref SequenceReader<byte> reader)
        {
            if (ProxyInfo == null)
            {
                _originalFilterContext = Context;
                Context = ProxyInfo = new ProxyInfo();
            }

            return base.Filter(ref reader);
        }
    }
}