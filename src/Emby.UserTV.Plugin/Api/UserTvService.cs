using System;
using Emby.UserTV.Plugin.Configuration;
using Emby.UserTV.Plugin.Services;
using MediaBrowser.Controller.Api;
using MediaBrowser.Controller.Net;

namespace Emby.UserTV.Plugin.Api
{
    public sealed class UserTvService : BaseApiService
    {
        public object Get(GetUserTvStatus request)
        {
            return new UserTvStatusResponse
            {
                RuntimeInitialized = UserTvRuntimeHost.IsInitialized,
                PluginVersion = typeof(Plugin).Assembly.GetName().Version?.ToString(),
                Mode = "StandalonePluginV0",
                WriteSupport = "DryRunPlanningOnly; write adapters are intentionally gated for later validation.",
                LastRun = UserTvRuntimeHost.Runtime?.LastRun
            };
        }

        public object Get(GetUserTvConfiguration request)
        {
            RequireAdministrator();
            return new UserTvConfigurationResponse
            {
                Configuration = Plugin.Instance?.Configuration ?? new PluginConfiguration()
            };
        }

        public object Post(UpdateUserTvConfiguration request)
        {
            RequireAdministrator();
            var plugin = Plugin.Instance;
            if (plugin == null)
            {
                throw new InvalidOperationException("Plugin instance is not available.");
            }

            var configuration = plugin.Configuration;
            configuration.EnableAutomaticPlanning = request?.EnableAutomaticPlanning ?? false;
            configuration.PlanningIntervalMinutes = Bound(request?.PlanningIntervalMinutes ?? 360, 15, 10080);
            configuration.EnableExperimentalWrites = request?.EnableExperimentalWrites ?? false;
            configuration.PlaylistPrefix = Normalize(request?.PlaylistPrefix, "UserTV");
            configuration.ChannelNameTemplate = Normalize(request?.ChannelNameTemplate, "{prefix} - {user}");
            configuration.MinimumItemsPerUser = Bound(request?.MinimumItemsPerUser ?? 3, 1, 1000);
            configuration.MaximumItemsPerUser = Bound(request?.MaximumItemsPerUser ?? 250, configuration.MinimumItemsPerUser, 5000);
            configuration.IncludeSeriesEpisodes = request?.IncludeSeriesEpisodes ?? true;
            configuration.AnonymizeDashboardExamples = request?.AnonymizeDashboardExamples ?? true;

            plugin.SaveConfiguration();

            return new UserTvConfigurationResponse
            {
                Configuration = configuration
            };
        }

        public object Post(CreateUserTvPlan request)
        {
            RequireAdministrator();
            var runtime = UserTvRuntimeHost.Runtime;
            if (runtime == null)
            {
                throw new InvalidOperationException("UserTV runtime is not initialized.");
            }

            return new UserTvRunResponse
            {
                Summary = runtime.Run(request == null || request.DryRun)
            };
        }

        private AuthorizationInfo RequireAdministrator()
        {
            var auth = AuthorizationContext?.GetAuthorizationInfo(Request);
            if (auth == null || auth.User == null || auth.UserId <= 0 || auth.User.Policy == null || !auth.User.Policy.IsAdministrator)
            {
                throw new UnauthorizedAccessException("Administrator privileges are required.");
            }

            return auth;
        }

        private static int Bound(int value, int minimum, int maximum)
        {
            return Math.Max(minimum, Math.Min(value, maximum));
        }

        private static string Normalize(string value, string fallback)
        {
            var normalized = (value ?? string.Empty).Trim();
            return normalized.Length == 0 ? fallback : normalized;
        }
    }
}
