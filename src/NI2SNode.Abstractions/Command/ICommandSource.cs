namespace NI2S.Node.Command
{
    public interface ICommandSource
    {
        IEnumerable<Type?> GetCommandTypes(Predicate<Type?> criteria);
    }
}
