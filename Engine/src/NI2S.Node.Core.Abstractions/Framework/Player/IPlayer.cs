using System.Threading.Tasks;

namespace NI2S.Node.Framework
{
    /// <summary>
    /// A player is, well, there's really no other good name...
    /// </summary>
    public interface IPlayerGrain : IActorGrain
    {
        // Each player is located in exactly one scene
        Task SetPlayerScene(ISceneGrain scene);
        Task<ISceneGrain> PlayerScene();

        // Until Death comes knocking
        Task Die();

        // A Player takes his turn by calling Play with a command
        Task<string?> Play(string command);
    }
}
