using System.Threading.Tasks;

namespace NI2S.Node
{
    public interface IChannelRegister
    {
        Task RegisterChannel(object connection);
    }
}