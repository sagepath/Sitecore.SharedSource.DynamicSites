using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.DynamicSites.Sites;
using Sitecore.SharedSource.DynamicSites.Utilities;
using Sitecore.Sites;

namespace Sitecore.SharedSource.DynamicSites.Events
{
    public class PublishEndRemoteHandler
    {
        internal void ClearSiteCache(object sender, EventArgs args)
        {
            if (DynamicSiteSettings.Disabled) return;
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            var remoteEventArgs = args as PublishEndRemoteEventArgs;
            if (!string.IsNullOrWhiteSpace(remoteEventArgs?.TargetDatabaseName))
            {
                var database = Database.GetDatabase(remoteEventArgs.TargetDatabaseName);
                if (database != null)
                {
                    var rootItemPublished = database.GetItem(new ID(remoteEventArgs.RootItemId));
                    if (DynamicSiteManager.HasBaseTemplate(rootItemPublished))
                    {
                        Log.Info("Clearing the dynamic site cache, invoked by remote:publish:end event", this);
                        DynamicSiteManager.ClearCache();

                        var providersCollection = SiteManager.Providers;
                        foreach (var provider in providersCollection)
                        {
                            if (provider is DynamicSitesProvider)
                            {
                                Log.Info("Re-initializing the dynamic sites", this);
                                (provider as DynamicSitesProvider).GetSites();
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
