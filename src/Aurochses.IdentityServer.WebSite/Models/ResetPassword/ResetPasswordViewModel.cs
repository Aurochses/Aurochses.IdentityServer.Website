using System.ComponentModel.DataAnnotations;

namespace Aurochses.IdentityServer.WebSite.Models.ResetPassword
{
    /// <summary>
    /// Class ResetPasswordViewModel.
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Email.Prompt", Description = "Email.Description")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Password.Prompt", Description = "Password.Description")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>The confirm password.</value>
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", Prompt = "ConfirmPassword.Prompt", Description = "ConfirmPassword.Description")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }
    }
}