namespace ARWNI2S.Engine.Core.Internal
{
    internal interface IThreadManager
    {
        Thread GetDedicatedInnerThread(IEngineProcessor processor);
        Thread AcquireThread();
        Thread ReleaseThread();
    }
}