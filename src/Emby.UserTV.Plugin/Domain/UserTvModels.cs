using System;
using System.Collections.Generic;

namespace Emby.UserTV.Plugin.Domain
{
    public sealed class UserTvSourceUser
    {
        public string UserId { get; set; }

        public string DisplayName { get; set; }

        public List<UserTvSourceItem> FavoriteItems { get; set; } = new List<UserTvSourceItem>();
    }

    public sealed class UserTvSourceItem
    {
        public string ItemId { get; set; }

        public string Title { get; set; }

        public string ItemType { get; set; }

        public long? RuntimeTicks { get; set; }
    }

    public sealed class UserTvPlan
    {
        public string UserId { get; set; }

        public string DisplayName { get; set; }

        public string SafeName { get; set; }

        public string PlaylistName { get; set; }

        public string ChannelName { get; set; }

        public bool IsEligible { get; set; }

        public string Reason { get; set; }

        public List<UserTvPlannedItem> Items { get; set; } = new List<UserTvPlannedItem>();
    }

    public sealed class UserTvPlannedItem
    {
        public string ItemId { get; set; }

        public string Title { get; set; }

        public string ItemType { get; set; }

        public int Order { get; set; }

        public long? RuntimeTicks { get; set; }
    }

    public sealed class UserTvRunSummary
    {
        public DateTimeOffset StartedAt { get; set; }

        public DateTimeOffset FinishedAt { get; set; }

        public bool DryRun { get; set; }

        public bool WritesEnabled { get; set; }

        public int UsersScanned { get; set; }

        public int EligibleUsers { get; set; }

        public int PlannedItems { get; set; }

        public string Mode { get; set; }

        public string Message { get; set; }

        public List<UserTvPlan> Plans { get; set; } = new List<UserTvPlan>();
    }
}
