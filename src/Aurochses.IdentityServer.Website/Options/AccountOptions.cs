namespace Aurochses.IdentityServer.Website.Options
{
    public class AccountOptions
    {
        public bool AllowLocalLogin { get; set; }
        public bool AllowRememberLogin { get; set; }
        public string WindowsAuthenticationSchemeName { get; set; }
    }
}