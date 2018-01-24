using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.DynamicSites.Utilities;
using Sitecore.Sites;

namespace Sitecore.SharedSource.DynamicSites.Sites
{
    public class SiteProviderUtil
    {
        public static void RefreshDynamicSites()
        {
            var database = DynamicSiteSettings.GetCurrentDatabase;

            if (database != null)
            {
                var providersCollection = SiteManager.Providers;
                foreach (var provider in providersCollection)
                {
                    if (provider is DynamicSitesProvider)
                    {
                        Log.Info("Re-initializing the dynamic sites", typeof(SiteProviderUtil));
                        (provider as DynamicSitesProvider).GetSites();
                        break;
                    }
                }
            }
        }
    }
}
