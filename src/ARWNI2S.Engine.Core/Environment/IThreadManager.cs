namespace ARWNI2S.Engine.Environment
{
    public interface IThreadManager
    {
        Thread DedicatedThread { get; }

        void End();
        void Run();
    }

    public interface IThreadManager<T> : IThreadManager
    {
    }

}