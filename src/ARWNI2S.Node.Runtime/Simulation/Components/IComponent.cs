namespace ARWNI2S.Runtime.Simulation.Components
{
    public interface IComponent
    {
        IComposite Owner { get; }
        IComponent ComponentRoot { get; }
        IComponent Parent { get; }
    }
}
