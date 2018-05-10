using Sitecore.Commerce.XA.Foundation.Common;
using Sitecore.Commerce.XA.Foundation.Common.UrlManager;
using System;
using System.Globalization;

namespace Sitecore.Support.Commerce.XA.Foundation.Common.UrlManager
{
  public class StorefrontUrlManager : Sitecore.Commerce.XA.Foundation.Common.UrlManager.StorefrontUrlManager
  {
    public StorefrontUrlManager(ISiteContext siteContext) : base(siteContext)
    {
    }

    public override UrlBuilder GetStorefrontUrl(string url)
    {
      string text;
      try
      {
        return new UrlBuilder(new Uri(url));
      }
      catch (Exception)
      {
        text = string.Format(CultureInfo.InvariantCulture, "/{0}", url.Trim(new char[]
        {
          '/'
        }));
      }
      if (!string.IsNullOrEmpty(base.SiteContext.VirtualFolder.Trim(new char[]
      {
        '/'
      })) && !text.Trim(new char[]
      {
        '/'
      }).StartsWith(base.SiteContext.VirtualFolder.Trim(new char[]
      {
        '/'
      }), StringComparison.OrdinalIgnoreCase) && !text.StartsWith(string.Format(CultureInfo.InvariantCulture, "/{0}/", base.SiteContext.VirtualFolder.Trim(new char[]
      {
        '/'
      })), StringComparison.OrdinalIgnoreCase))
      {
        text = string.Format(CultureInfo.InvariantCulture, "/{0}/{1}", base.SiteContext.VirtualFolder.Trim(new char[]
        {
          '/'
        }), text.Trim(new char[]
        {
          '/'
        }));
      }
      string hostName = Context.Site.HostName;
      string uriString;
      if (!string.IsNullOrEmpty(hostName) && !hostName.Contains("*") && !hostName.Contains("|"))
      {
        uriString = string.Format(CultureInfo.InvariantCulture, "{0}://{1}", StorefrontUrlManager.CurrentUrl.Scheme, hostName);
      }
      else
      {
        uriString = string.Format(CultureInfo.InvariantCulture, "{0}://{1}", StorefrontUrlManager.CurrentUrl.Scheme, StorefrontUrlManager.CurrentUrl.Host);
      }
      return new UrlBuilder(new Uri(new Uri(uriString), text));
    }
  }
}
