using ARWNI2S.Engine.Users.Data;
using System.IdentityModel.Tokens.Jwt;

namespace ARWNI2S.Engine.Security
{
    public interface IUserTokenService
    {
        string SerializeToken(User user);
        string SerializeToken(JwtSecurityToken token);
        JwtSecurityToken GenerateToken(User user);
    }
}
