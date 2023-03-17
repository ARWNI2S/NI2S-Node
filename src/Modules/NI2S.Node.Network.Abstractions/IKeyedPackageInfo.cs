namespace NI2S.Node.Networking
{
    public interface IKeyedPackageInfo<TKey>
    {
        TKey Key { get; }
    }
}
