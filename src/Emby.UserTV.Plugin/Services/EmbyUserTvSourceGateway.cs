using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Querying;
using Emby.UserTV.Plugin.Domain;

namespace Emby.UserTV.Plugin.Services
{
    public sealed class EmbyUserTvSourceGateway : IUserTvSourceGateway
    {
        private static readonly string[] SourceItemTypes = { "Movie", "Episode", "Series", "Video" };

        private readonly ILibraryManager _libraryManager;
        private readonly IUserManager _userManager;
        private readonly ILogger _logger;

        public EmbyUserTvSourceGateway(ILibraryManager libraryManager, IUserManager userManager, ILogger logger)
        {
            _libraryManager = libraryManager ?? throw new ArgumentNullException(nameof(libraryManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger;
        }

        public List<UserTvSourceUser> LoadUsersWithFavorites()
        {
            return (_userManager.GetUserList(new UserQuery()) ?? Array.Empty<User>())
                .Where(user => user != null)
                .Select(LoadUser)
                .ToList();
        }

        private UserTvSourceUser LoadUser(User user)
        {
            try
            {
                var query = new InternalItemsQuery
                {
                    User = user,
                    Recursive = true,
                    IncludeItemTypes = SourceItemTypes,
                    IsFavorite = true,
                    IsVirtualItem = false,
                    EnableTotalRecordCount = false,
                    EnforceContentRestriction = true,
                    OrderBy = new[] { ("SortName", SortOrder.Ascending) }
                };

                query.SetUser(user);

                var result = _libraryManager.GetItemList(query) ?? Array.Empty<BaseItem>();
                var favorites = result
                    .Where(item => item != null && item.IsVisible(user))
                    .Select(ToSourceItem)
                    .ToList();

                return new UserTvSourceUser
                {
                    UserId = user.InternalId.ToString(CultureInfo.InvariantCulture),
                    DisplayName = user.Name,
                    FavoriteItems = favorites
                };
            }
            catch (Exception ex)
            {
                _logger?.ErrorException("[UserTV] Failed to load favorites for user {0}.", ex, user.Name);
                return new UserTvSourceUser
                {
                    UserId = user.InternalId.ToString(CultureInfo.InvariantCulture),
                    DisplayName = user.Name,
                    FavoriteItems = new List<UserTvSourceItem>()
                };
            }
        }

        private static UserTvSourceItem ToSourceItem(BaseItem item)
        {
            var clientType = item.GetClientTypeName();
            return new UserTvSourceItem
            {
                ItemId = item.InternalId.ToString(CultureInfo.InvariantCulture),
                Title = item.Name,
                ItemType = string.IsNullOrWhiteSpace(clientType) ? item.GetType().Name : clientType,
                RuntimeTicks = item.RunTimeTicks
            };
        }
    }
}
