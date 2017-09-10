using System.ComponentModel.DataAnnotations;

namespace Aurochses.IdentityServer.WebSite.Models.TwoFactorSignIn
{
    /// <summary>
    /// Class VerifyCodeViewModel.
    /// </summary>
    public class VerifyCodeViewModel
    {
        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        /// <value>The provider.</value>
        [Required]
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        [Required]
        [Display(Name = "Token", Prompt = "Token.Prompt", Description = "Token.Description")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the return URL.
        /// </summary>
        /// <value>The return URL.</value>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [remember browser].
        /// </summary>
        /// <value><c>true</c> if [remember browser]; otherwise, <c>false</c>.</value>
        [Display(Name = "RememberBrowser")]
        public bool RememberBrowser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [remember me].
        /// </summary>
        /// <value><c>true</c> if [remember me]; otherwise, <c>false</c>.</value>
        public bool RememberMe { get; set; }
    }
}