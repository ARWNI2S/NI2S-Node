namespace NI2S.Node.Infrastructure
{
    public abstract class PlatformInfo<TSystemCaps>
        where TSystemCaps : PlatformInfo<TSystemCaps>, new()
    {
        private static TSystemCaps? _systemCaps = null;

        protected PlatformInfo()
        {
            ReadCapabilities();
        }

        protected abstract void ReadCapabilities();

        public static TSystemCaps GetCaps()
        {
            _systemCaps ??= new TSystemCaps();
            return _systemCaps;
        }
    }
}
