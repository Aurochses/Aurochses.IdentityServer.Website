using System.ComponentModel.DataAnnotations;

namespace Aurochses.IdentityServer.WebSite.Models.RemindPassword
{
    /// <summary>
    /// Class RemindPasswordViewModel.
    /// </summary>
    public class RemindPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Email.Prompt", Description = "Email.Description")]
        public string Email { get; set; }
    }
}