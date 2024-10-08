namespace ARWNI2S.Infrastructure.Timing
{
    internal interface ITimeInterval
    {
        void Start();

        void Stop();

        void Restart();

        TimeSpan Elapsed { get; }
    }
}
