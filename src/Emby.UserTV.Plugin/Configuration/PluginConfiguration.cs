using MediaBrowser.Model.Plugins;

namespace Emby.UserTV.Plugin.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public bool EnableAutomaticPlanning { get; set; } = false;

        public int PlanningIntervalMinutes { get; set; } = 360;

        public bool EnableExperimentalWrites { get; set; } = false;

        public string PlaylistPrefix { get; set; } = "UserTV";

        public string ChannelNameTemplate { get; set; } = "{prefix} - {user}";

        public int MinimumItemsPerUser { get; set; } = 3;

        public int MaximumItemsPerUser { get; set; } = 250;

        public bool IncludeSeriesEpisodes { get; set; } = true;

        public bool AnonymizeDashboardExamples { get; set; } = true;
    }
}
