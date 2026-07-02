using System;
using System.Threading;
using Emby.UserTV.Plugin.Configuration;
using Emby.UserTV.Plugin.Services;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Model.Logging;

namespace Emby.UserTV.Plugin
{
    public sealed class ServerEntryPoint : IServerEntryPoint
    {
        private readonly ILibraryManager _libraryManager;
        private readonly IUserManager _userManager;
        private readonly ILogger _logger;
        private Timer _planningTimer;
        private int _planningInProgress;

        public ServerEntryPoint(ILibraryManager libraryManager, IUserManager userManager, ILogManager logManager)
        {
            _libraryManager = libraryManager ?? throw new ArgumentNullException(nameof(libraryManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logManager.GetLogger(Constants.PluginName);
        }

        public void Run()
        {
            var gateway = new EmbyUserTvSourceGateway(_libraryManager, _userManager, _logger);
            var planner = new UserTvPlanner();
            var runtime = new UserTvRuntime(gateway, planner, _logger);
            UserTvRuntimeHost.Initialize(runtime);

            var configuration = Plugin.Instance?.Configuration ?? new PluginConfiguration();
            if (configuration.EnableAutomaticPlanning)
            {
                var interval = TimeSpan.FromMinutes(Math.Max(15, configuration.PlanningIntervalMinutes));
                _planningTimer = new Timer(_ => RunScheduledPlanning(), null, interval, interval);
            }

            _logger.Info("[UserTV] Runtime initialized.");
        }

        public void Dispose()
        {
            _planningTimer?.Dispose();
            _planningTimer = null;
            UserTvRuntimeHost.Shutdown();
            _logger.Info("[UserTV] Runtime stopped.");
        }

        private void RunScheduledPlanning()
        {
            if (Interlocked.Exchange(ref _planningInProgress, 1) == 1)
            {
                return;
            }

            try
            {
                UserTvRuntimeHost.Runtime?.Run(true);
            }
            catch (Exception ex)
            {
                _logger.ErrorException("[UserTV] Scheduled planning failed.", ex);
            }
            finally
            {
                Interlocked.Exchange(ref _planningInProgress, 0);
            }
        }
    }
}
