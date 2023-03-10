namespace NI2S.Node.Protocol.Channel
{
    public static class Extensions
    {
        public static IAsyncEnumerator<TPackageInfo> GetPackageStream<TPackageInfo>(this IChannel<TPackageInfo> channel)
            where TPackageInfo : class
        {
            return channel.RunAsync().GetAsyncEnumerator();
        }

        public static async ValueTask<TPackageInfo> ReceiveAsync<TPackageInfo>(this IAsyncEnumerator<TPackageInfo> packageStream)
        {
            if (await packageStream.MoveNextAsync())
                return packageStream.Current;

            //TODO: TPackageInfo to deterministic packages
            return default!;
        }
    }
}
