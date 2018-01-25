using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Events;
using Sitecore.Events;
using Sitecore.SharedSource.DynamicSites.Utilities;

namespace Sitecore.SharedSource.DynamicSites.Events
{
    public class ItemRemoteHandler
    {
        [UsedImplicitly]
        internal void OnItemDeletedRemote(object sender, EventArgs args)
        {
            if (DynamicSiteSettings.Disabled) return;
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            var remoteArgs = args as ItemDeletedRemoteEventArgs;

            if (remoteArgs?.Item != null)
            {
                //If item being deleted is a Dynamic Site Definition item, queue the event to refresh dyamic sites on any remote servers
                if (DynamicSiteManager.HasBaseTemplate(remoteArgs.Item))
                {
                    var refreshDynamicSitesEvent = new RefreshDynamicSitesEvent(remoteArgs.Item.ID.Guid);
                    Log.Info("Queueing refreshDynamicSites:remote event from the OnItemDeletedRemote method", this);
                    Event.RaiseEvent("refreshDynamicSites:remote", refreshDynamicSitesEvent);
                }
            }
        }
    }
}
