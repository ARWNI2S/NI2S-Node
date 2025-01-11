// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.


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