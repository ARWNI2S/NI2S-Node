using ARWNI2S.Data.Entities.Users;
using ARWNI2S.Services.Caching;

namespace ARWNI2S.Services.Users.Caching
{
    /// <summary>
    /// Represents a user password cache event consumer
    /// </summary>
    public partial class UserPasswordCacheEventConsumer : DataCacheEventConsumer<UserPassword>
    {
    }
}