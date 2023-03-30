using Orleans;
using System.Threading.Tasks;

namespace NI2S.Node.Framework
{
    /// <summary>
    /// A room is any location in a game, including outdoor locations and
    /// spaces that are arguably better described as moist, cold, caverns.
    /// </summary>
    public interface ISceneGrain : IGrainWithIntegerKey
    {
        // Rooms have a textual description
        Task<string> Description(PlayerInfo whoisAsking);
        Task SetInfo(PlayerInfo info);

        Task<ISceneGrain> ExitTo(string direction);

        // Players can enter or exit a room
        Task Enter(PlayerInfo player);
        Task Exit(PlayerInfo player);

        // Character can enter or exit a room
        Task Enter(CharacterInfo character);
        Task Exit(CharacterInfo character);

        // Things can be dropped or taken from a room
        Task Drop(Thing thing);
        Task Take(Thing thing);
        Task<Thing> FindThing(string name);

        // Players and character can be killed, if you have the right weapon.
        Task<PlayerInfo> FindPlayer(string name);
        Task<CharacterInfo> FindCharacter(string name);
    }
}
