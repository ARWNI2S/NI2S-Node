using ARWNI2S.Node.Data.Entities.Users;
using ARWNI2S.Node.Services.Caching;

namespace ARWNI2S.Node.Services.Users.Caching
{
    /// <summary>
    /// Represents a user password cache notification consumer
    /// </summary>
    public partial class UserPasswordCacheConsumer : CacheConsumer<UserPassword>
    {
    }
}