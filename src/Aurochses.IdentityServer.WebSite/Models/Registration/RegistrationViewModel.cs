using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Aurochses.IdentityServer.WebSite.Models.Registration
{
    /// <summary>
    /// Class RegistrationViewModel.
    /// </summary>
    public class RegistrationViewModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [EmailAddress]
        [Remote("ValidateEmail", "Registration", AdditionalFields = "__RequestVerificationToken", HttpMethod = "Post", ErrorMessage = "UserAlreadyExists")]
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
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "FirstName", Prompt = "FirstName.Prompt", Description = "FirstName.Description")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "LastName", Prompt = "LastName.Prompt", Description = "LastName.Description")]
        public string LastName { get; set; }
    }
}