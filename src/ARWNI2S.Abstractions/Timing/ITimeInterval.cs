namespace ARWNI2S.Timing
{
    internal interface ITimeInterval
    {
        void Start();

        void Stop();

        void Restart();

        TimeSpan Elapsed { get; }
    }
}
