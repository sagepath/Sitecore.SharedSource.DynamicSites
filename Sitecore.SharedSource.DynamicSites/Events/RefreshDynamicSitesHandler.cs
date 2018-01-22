using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.DynamicSites.Sites;
using Sitecore.SharedSource.DynamicSites.Utilities;
using Sitecore.Sites;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Events.Hooks;

namespace Sitecore.SharedSource.DynamicSites.Events
{
    public class RefreshDynamicSitesHandler
    {
        [UsedImplicitly]
        public void OnRefreshDynamicSites(object sender, EventArgs args)
        {
            if (DynamicSiteSettings.Disabled) return;
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            var siteDefinitionId = ((args as SitecoreEventArgs)?.Parameters[0] as RefreshDynamicSitesEvent)?.SiteDefinitionItemId;
            if (siteDefinitionId == null) return;

            var database = DynamicSiteSettings.GetCurrentDatabase;
                
            if (database != null)
            {
                Log.Info("Clearing the dynamic site cache", this);
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

    [DataContract]
    public class RefreshDynamicSitesEvent
    {
        public RefreshDynamicSitesEvent() { }
        public RefreshDynamicSitesEvent(Guid siteDefinitionItemId)
        {
            SiteDefinitionItemId = siteDefinitionItemId;
        }

        [DataMember]
        public Guid SiteDefinitionItemId { get; protected set; }
    }
}
