namespace BilligKwhWebApp.Core.Interfaces
{
    public partial interface IWebHelper
    {
        string GetUrlReferrer();

        string GetCurrentIpAddress();

        string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false);
    }
}
