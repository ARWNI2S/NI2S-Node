using ARWNI2S.Node.Data.Entities.Users;
using ARWNI2S.Node.Services.Caching;

namespace ARWNI2S.Node.Services.Users.Caching
{
    /// <summary>
    /// Represents a user password cache event consumer
    /// </summary>
    public partial class UserPasswordCacheEventConsumer : CacheEventConsumer<UserPassword>
    {
    }
}