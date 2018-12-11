namespace Sitecore.Support.Commerce.XA.Foundation.Common.UrlManager
{
  using Sitecore.Commerce.XA.Foundation.Common.UrlManager;
  using System;
  using System.Globalization;
  using System.Runtime.Remoting.Contexts;
  using Sitecore.Commerce.XA.Foundation.Common.Context;
  public class StorefrontUrlManager : Sitecore.Commerce.XA.Foundation.Common.UrlManager.StorefrontUrlManager
  {
    public StorefrontUrlManager(ISiteContext siteContext) : base(siteContext)
    {
    }

    public override UrlBuilder GetStorefrontUrl(string url)
    {
      Uri storefrontUri;
      if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
      {
        storefrontUri = new Uri(url);
        return new UrlBuilder(storefrontUri);
      }

      var path = string.Format(CultureInfo.InvariantCulture, "/{0}", url.Trim('/'));

      if (!string.IsNullOrEmpty(this.SiteContext.VirtualFolder.Trim('/')))
      {
        if (!(path + "/").StartsWith(
            string.Format(CultureInfo.InvariantCulture, "/{0}/", this.SiteContext.VirtualFolder.Trim('/')),
            StringComparison.OrdinalIgnoreCase))
        {
          path = string.Format(CultureInfo.InvariantCulture, "/{0}/{1}", this.SiteContext.VirtualFolder.Trim('/'), path.Trim('/'));
        }
      }

      if (this.IncludeLanguage)
      {
        path = string.Format(CultureInfo.InvariantCulture, "/{0}/{1}", Sitecore.Context.Language.Name, path.Trim('/'));
      }

      var currentSiteHostName = Sitecore.Context.Site.HostName;
      string baseUrl;
      if (this.IsValidHostName(currentSiteHostName))
      {
        baseUrl = string.Format(CultureInfo.InvariantCulture, "{0}://{1}", CurrentUrl.Scheme, currentSiteHostName);
      }
      else
      {
        baseUrl = string.Format(CultureInfo.InvariantCulture, "{0}://{1}", CurrentUrl.Scheme, CurrentUrl.Host);
      }

      Uri baseUri = new Uri(baseUrl);
      storefrontUri = new Uri(baseUri, path);
      return new UrlBuilder(storefrontUri);
    }

    /// <summary>
    /// Checks if parameter is valid hostName.
    /// </summary>
    /// <param name="hostName">
    /// The current site host name.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    /// <remarks>Currently verifies scenarios with * or | only.</remarks>
    protected virtual bool IsValidHostName(string hostName)
    {
      if (string.IsNullOrEmpty(hostName))
      {
        return false;
      }

      if (hostName.Contains("*"))
      {
        return false;
      }

      return !hostName.Contains("|");
    }
  }
}
