using System.ComponentModel.DataAnnotations;

namespace Aurochses.IdentityServer.WebSite.Models.SignIn
{
    /// <summary>
    /// Class SignInViewModel.
    /// </summary>
    public class SignInViewModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Email.Prompt")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Password.Prompt")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [remember me].
        /// </summary>
        /// <value><c>true</c> if [remember me]; otherwise, <c>false</c>.</value>
        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}