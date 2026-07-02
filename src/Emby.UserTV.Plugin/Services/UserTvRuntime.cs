using System;
using System.Linq;
using Emby.UserTV.Plugin.Configuration;
using Emby.UserTV.Plugin.Domain;
using MediaBrowser.Model.Logging;

namespace Emby.UserTV.Plugin.Services
{
    public sealed class UserTvRuntime
    {
        private readonly IUserTvSourceGateway _sourceGateway;
        private readonly UserTvPlanner _planner;
        private readonly ILogger _logger;
        private readonly object _syncRoot = new object();
        private UserTvRunSummary _lastRun;

        public UserTvRuntime(IUserTvSourceGateway sourceGateway, UserTvPlanner planner, ILogger logger)
        {
            _sourceGateway = sourceGateway ?? throw new ArgumentNullException(nameof(sourceGateway));
            _planner = planner ?? throw new ArgumentNullException(nameof(planner));
            _logger = logger;
        }

        public UserTvRunSummary LastRun
        {
            get
            {
                lock (_syncRoot)
                {
                    return _lastRun;
                }
            }
        }

        public UserTvRunSummary Run(bool dryRun)
        {
            var startedAt = DateTimeOffset.UtcNow;
            var configuration = Plugin.Instance?.Configuration ?? new PluginConfiguration();
            var effectiveDryRun = dryRun || !configuration.EnableExperimentalWrites;
            var users = _sourceGateway.LoadUsersWithFavorites();
            var plans = _planner.BuildPlans(users, configuration);
            var eligible = plans.Where(plan => plan.IsEligible).ToList();

            var summary = new UserTvRunSummary
            {
                StartedAt = startedAt,
                FinishedAt = DateTimeOffset.UtcNow,
                DryRun = effectiveDryRun,
                WritesEnabled = configuration.EnableExperimentalWrites,
                UsersScanned = plans.Count,
                EligibleUsers = eligible.Count,
                PlannedItems = eligible.Sum(plan => plan.Items.Count),
                Mode = effectiveDryRun ? "dry-run" : "experimental-write-gate",
                Message = effectiveDryRun
                    ? "Plans were calculated only. No Emby playlists or VirtualTV channels were changed."
                    : "Write mode is enabled, but playlist and VirtualTV write adapters are not activated in this V0 plugin build.",
                Plans = plans
            };

            lock (_syncRoot)
            {
                _lastRun = summary;
            }

            _logger?.Info("[UserTV] Planning finished. Users: {0}, eligible: {1}, items: {2}, mode: {3}.",
                summary.UsersScanned,
                summary.EligibleUsers,
                summary.PlannedItems,
                summary.Mode);

            return summary;
        }
    }
}
