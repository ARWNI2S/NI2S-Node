namespace ARWNI2S.Infrastructure.Network.Protocol
{
    public interface IKeyedPackageInfo<TKey>
    {
        TKey Key { get; }
    }
}
