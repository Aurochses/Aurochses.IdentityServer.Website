using System.ComponentModel.DataAnnotations;

namespace Aurochses.IdentityServer.WebSite.Models.EmailConfirmation
{
    /// <summary>
    /// Class EmailConfirmationViewModel.
    /// </summary>
    public class EmailConfirmationViewModel
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