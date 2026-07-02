using System;

namespace Emby.UserTV.Plugin.Services
{
    public static class UserTvRuntimeHost
    {
        private static readonly object SyncRoot = new object();

        public static UserTvRuntime Runtime { get; private set; }

        public static bool IsInitialized => Runtime != null;

        public static void Initialize(UserTvRuntime runtime)
        {
            if (runtime == null)
            {
                throw new ArgumentNullException(nameof(runtime));
            }

            lock (SyncRoot)
            {
                Runtime = runtime;
            }
        }

        public static void Shutdown()
        {
            lock (SyncRoot)
            {
                Runtime = null;
            }
        }
    }
}
