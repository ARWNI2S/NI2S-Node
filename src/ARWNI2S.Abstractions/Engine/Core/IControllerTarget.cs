namespace ARWNI2S.Engine.Core
{
    public interface IControllerTarget
    {
        IController Controller { get; }

        void SetController(IController controller);
    }
}