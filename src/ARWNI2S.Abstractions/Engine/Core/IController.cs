namespace ARWNI2S.Engine.Core
{
    public interface IController
    {
        INiisActor ControlledActor { get; }

        void AssumeControl(INiisActor actor);

        void DropControl();
    }
}