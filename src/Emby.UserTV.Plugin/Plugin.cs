using System;
using System.Collections.Generic;
using Emby.UserTV.Plugin.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Emby.UserTV.Plugin
{
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        public static Plugin Instance { get; private set; }

        public override string Name => Constants.PluginName;

        public override string Description => Constants.PluginDescription;

        public override Guid Id => Constants.PluginId;

        public IEnumerable<PluginPageInfo> GetPages()
        {
            var ns = GetType().Namespace;

            return new[]
            {
                new PluginPageInfo
                {
                    Name = "usertv",
                    DisplayName = "UserTV Stream",
                    EmbeddedResourcePath = ns + ".Web.index.html",
                    EnableInMainMenu = true
                },
                new PluginPageInfo
                {
                    Name = "usertvjs",
                    EmbeddedResourcePath = ns + ".Web.index.js"
                },
                new PluginPageInfo
                {
                    Name = "usertvcss",
                    EmbeddedResourcePath = ns + ".Web.styles.css"
                }
            };
        }
    }
}
