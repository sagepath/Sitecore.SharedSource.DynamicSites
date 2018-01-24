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

namespace Sitecore.SharedSource.DynamicSites.Events
{
    public class PublishEndHandler
    {
        [UsedImplicitly]
        internal void OnPublishEnd(object sender, EventArgs args)
        {
            if (DynamicSiteSettings.Disabled) return;
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            var publisher = Event.ExtractParameter(args, 0) as Publisher;
            Error.AssertObject(publisher, "Publisher");

            var rootItemPublished = publisher?.Options.RootItem;

            if (rootItemPublished != null)
            {
                if (DynamicSiteManager.HasBaseTemplate(rootItemPublished))
                {
                    Log.Info("Clearing the dynamic site cache", this);
                    DynamicSiteManager.ClearCache();

                    SiteProviderUtil.RefreshDynamicSites();
                }
            }
        }
    }
}
