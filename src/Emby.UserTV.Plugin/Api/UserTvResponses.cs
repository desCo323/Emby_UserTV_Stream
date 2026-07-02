using Emby.UserTV.Plugin.Configuration;
using Emby.UserTV.Plugin.Domain;

namespace Emby.UserTV.Plugin.Api
{
    public abstract class UserTvResponse
    {
        public bool Success { get; set; } = true;
    }

    public sealed class UserTvStatusResponse : UserTvResponse
    {
        public bool RuntimeInitialized { get; set; }

        public string PluginVersion { get; set; }

        public string Mode { get; set; }

        public string WriteSupport { get; set; }

        public UserTvRunSummary LastRun { get; set; }
    }

    public sealed class UserTvConfigurationResponse : UserTvResponse
    {
        public PluginConfiguration Configuration { get; set; }
    }

    public sealed class UserTvRunResponse : UserTvResponse
    {
        public UserTvRunSummary Summary { get; set; }
    }
}
