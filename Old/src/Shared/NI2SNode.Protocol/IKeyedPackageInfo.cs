namespace NI2S.Node.Protocol
{
    public interface IKeyedPackageInfo<TKey>
    {
        TKey Key { get; }
    }
}
