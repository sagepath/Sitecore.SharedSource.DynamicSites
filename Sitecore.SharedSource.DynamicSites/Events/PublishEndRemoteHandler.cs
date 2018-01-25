using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Publishing;
using Sitecore.SharedSource.DynamicSites.Items.ModuleSettings;
using Sitecore.SharedSource.DynamicSites.Sites;
using Sitecore.SharedSource.DynamicSites.Utilities;
using Sitecore.Sites;

namespace Sitecore.SharedSource.DynamicSites.Events
{
    public class PublishEndRemoteHandler
    {
        [UsedImplicitly]
        internal void OnPublishEndRemote(object sender, EventArgs args)
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
                        var refreshDynamicSitesEvent = new RefreshDynamicSitesEvent(rootItemPublished.ID.Guid);
                        Log.Info("Queueing refreshDynamicSites:remote event from the OnPublishEndRemote method", this);
                        Event.RaiseEvent("refreshDynamicSites:remote", refreshDynamicSitesEvent);
                    }
                }
            }
        }
    }
}
