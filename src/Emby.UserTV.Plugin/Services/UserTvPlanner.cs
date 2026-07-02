using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Emby.UserTV.Plugin.Configuration;
using Emby.UserTV.Plugin.Domain;

namespace Emby.UserTV.Plugin.Services
{
    public sealed class UserTvPlanner
    {
        private static readonly Regex UnsafeNameCharacters = new Regex(@"[^A-Za-z0-9._ -]+", RegexOptions.Compiled);

        public List<UserTvPlan> BuildPlans(IEnumerable<UserTvSourceUser> users, PluginConfiguration configuration)
        {
            var config = configuration ?? new PluginConfiguration();
            var minimumItems = Math.Max(1, config.MinimumItemsPerUser);
            var maximumItems = Math.Max(minimumItems, config.MaximumItemsPerUser);
            var prefix = NormalizeName(config.PlaylistPrefix, "UserTV");

            return (users ?? Enumerable.Empty<UserTvSourceUser>())
                .Where(user => user != null)
                .Select(user => BuildPlan(user, config, prefix, minimumItems, maximumItems))
                .ToList();
        }

        private static UserTvPlan BuildPlan(
            UserTvSourceUser user,
            PluginConfiguration configuration,
            string prefix,
            int minimumItems,
            int maximumItems)
        {
            var displayName = NormalizeName(user.DisplayName, "Emby User");
            var safeName = ToSafeName(displayName);
            var selectedItems = (user.FavoriteItems ?? new List<UserTvSourceItem>())
                .Where(item => item != null && !string.IsNullOrWhiteSpace(item.ItemId))
                .Take(maximumItems)
                .Select((item, index) => new UserTvPlannedItem
                {
                    ItemId = item.ItemId,
                    Title = NormalizeName(item.Title, "Untitled"),
                    ItemType = NormalizeName(item.ItemType, "Video"),
                    RuntimeTicks = item.RuntimeTicks,
                    Order = index + 1
                })
                .ToList();

            var playlistName = $"{prefix} - {safeName}";
            var channelName = ApplyChannelTemplate(configuration.ChannelNameTemplate, prefix, safeName);
            var isEligible = selectedItems.Count >= minimumItems;

            return new UserTvPlan
            {
                UserId = user.UserId,
                DisplayName = displayName,
                SafeName = safeName,
                PlaylistName = playlistName,
                ChannelName = channelName,
                IsEligible = isEligible,
                Reason = isEligible ? "ready" : $"minimum_items_not_met:{minimumItems}",
                Items = selectedItems
            };
        }

        private static string ApplyChannelTemplate(string template, string prefix, string safeName)
        {
            var value = string.IsNullOrWhiteSpace(template) ? "{prefix} - {user}" : template.Trim();
            value = value.Replace("{prefix}", prefix).Replace("{user}", safeName);
            return NormalizeName(value, $"{prefix} - {safeName}");
        }

        private static string ToSafeName(string value)
        {
            var normalized = UnsafeNameCharacters.Replace(NormalizeName(value, "user"), string.Empty).Trim();
            normalized = Regex.Replace(normalized, @"\s+", " ");
            return string.IsNullOrWhiteSpace(normalized) ? "user" : normalized;
        }

        private static string NormalizeName(string value, string fallback)
        {
            var normalized = (value ?? string.Empty).Trim();
            return normalized.Length == 0 ? fallback : normalized;
        }
    }
}
