using ARWNI2S.Node.Core.Entities.Users;
using System.IdentityModel.Tokens.Jwt;

namespace ARWNI2S.Node.Services.Security
{
    public interface IUserTokenService
    {
        string SerializeToken(User user);
        string SerializeToken(JwtSecurityToken token);
        JwtSecurityToken GenerateToken(User user);
    }
}
