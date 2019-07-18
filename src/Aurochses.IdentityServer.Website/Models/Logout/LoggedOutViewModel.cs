namespace Aurochses.IdentityServer.Website.Models.Logout
{
    public class LoggedOutViewModel
    {
        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
        public string SignOutIframeUrl { get; set; }

        public bool AutomaticRedirectAfterSignOut { get; set; } = false;

        public string LogoutId { get; set; }
        public bool TriggerExternalSignOut => !string.IsNullOrWhiteSpace(ExternalAuthenticationScheme);
        public string ExternalAuthenticationScheme { get; set; }
    }
}