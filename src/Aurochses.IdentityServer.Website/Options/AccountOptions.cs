namespace Aurochses.IdentityServer.Website.Options
{
    public class AccountOptions
    {
        public bool AllowLocalLogin { get; set; }
        public bool AllowRememberLogin { get; set; }
        public string WindowsAuthenticationSchemeName { get; set; }
        public bool LockoutOnFailure { get; set; }
        public bool RequireEmailConfirmation { get; set; }
        public bool AllowTwoFactorAuthentication { get; set; }

        public bool ShowLogoutPrompt { get; set; }
        public bool AutomaticRedirectAfterSignOut { get; set; }
    }
}