using System;

namespace NI2S.Node
{
    public interface ISessionFactory
    {
        IAppSession Create();

        Type SessionType { get; }
    }
}