using MediaBrowser.Model.Services;

namespace Emby.UserTV.Plugin.Api
{
    [Route("/usertv/status", "GET")]
    public sealed class GetUserTvStatus : IReturn<UserTvStatusResponse>
    {
    }

    [Route("/usertv/configuration", "GET")]
    public sealed class GetUserTvConfiguration : IReturn<UserTvConfigurationResponse>
    {
    }

    [Route("/usertv/configuration", "POST")]
    public sealed class UpdateUserTvConfiguration : IReturn<UserTvConfigurationResponse>
    {
        public bool EnableAutomaticPlanning { get; set; }

        public int PlanningIntervalMinutes { get; set; }

        public bool EnableExperimentalWrites { get; set; }

        public string PlaylistPrefix { get; set; }

        public string ChannelNameTemplate { get; set; }

        public int MinimumItemsPerUser { get; set; }

        public int MaximumItemsPerUser { get; set; }

        public bool IncludeSeriesEpisodes { get; set; }

        public bool AnonymizeDashboardExamples { get; set; }
    }

    [Route("/usertv/plan", "POST")]
    public sealed class CreateUserTvPlan : IReturn<UserTvRunResponse>
    {
        public bool DryRun { get; set; } = true;
    }
}
