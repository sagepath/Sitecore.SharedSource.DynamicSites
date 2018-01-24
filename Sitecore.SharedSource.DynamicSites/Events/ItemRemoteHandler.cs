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
        internal void OnItemSavedRemote(object sender, EventArgs args)
        {
            if (DynamicSiteSettings.Disabled) return;
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            var remoteArgs = args as ItemSavedRemoteEventArgs;

            if (remoteArgs != null)
            {
                //If item being deleted is a Dynamic Site Definition item, queue the event to refresh dyamic sites on any remote servers
                if (DynamicSiteManager.HasBaseTemplate(remoteArgs.Item))
                {
                    var refreshDynamicSitesEvent = new RefreshDynamicSitesEvent(remoteArgs.Item.ID.Guid);
                    Log.Info("Queueing refreshDynamicSites:remote event from the OnItemSavedRemote method", typeof(ItemRemoteHandler));
                    Event.RaiseEvent("refreshDynamicSites:remote", refreshDynamicSitesEvent);
                }
            }
        }

        [UsedImplicitly]
        internal void OnItemRenamedRemote(object sender, EventArgs args)
        {
            if (DynamicSiteSettings.Disabled) return;
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            var remoteArgs = args as ItemSavedRemoteEventArgs;

            if (remoteArgs != null)
            {
                //If item being deleted is a Dynamic Site Definition item, queue the event to refresh dyamic sites on any remote servers
                if (DynamicSiteManager.HasBaseTemplate(remoteArgs.Item))
                {
                    var refreshDynamicSitesEvent = new RefreshDynamicSitesEvent(remoteArgs.Item.ID.Guid);
                    Log.Info("Queueing refreshDynamicSites:remote event from the OnItemRenamedRemote method", typeof(ItemRemoteHandler));
                    Event.RaiseEvent("refreshDynamicSites:remote", refreshDynamicSitesEvent);
                }
            }
        }

        [UsedImplicitly]
        internal void OnItemDeletedRemote(object sender, EventArgs args)
        {
            if (DynamicSiteSettings.Disabled) return;
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            var remoteArgs = args as ItemDeletedRemoteEventArgs;

            if (remoteArgs != null)
            {
                //If item being deleted is a Dynamic Site Definition item, queue the event to refresh dyamic sites on any remote servers
                if (DynamicSiteManager.HasBaseTemplate(remoteArgs.Item))
                {
                    var refreshDynamicSitesEvent = new RefreshDynamicSitesEvent(remoteArgs.Item.ID.Guid);
                    Log.Info("Queueing refreshDynamicSites:remote event from the OnItemDeletedRemote method", typeof(ItemRemoteHandler));
                    Event.RaiseEvent("refreshDynamicSites:remote", refreshDynamicSitesEvent);
                }
            }
        }
    }
}
