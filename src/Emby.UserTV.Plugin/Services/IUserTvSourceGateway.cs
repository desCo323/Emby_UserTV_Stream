using System.Collections.Generic;
using Emby.UserTV.Plugin.Domain;

namespace Emby.UserTV.Plugin.Services
{
    public interface IUserTvSourceGateway
    {
        List<UserTvSourceUser> LoadUsersWithFavorites();
    }
}
