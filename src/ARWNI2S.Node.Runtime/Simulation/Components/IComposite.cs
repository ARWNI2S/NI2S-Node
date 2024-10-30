namespace ARWNI2S.Runtime.Simulation.Components
{
    public interface IComposite
    {
        IComponent ComponentRoot { get; }

        IEnumerable<IComponent> Components { get; }
    }
}
