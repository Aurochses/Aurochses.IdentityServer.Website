using System.ComponentModel.DataAnnotations;

namespace Aurochses.IdentityServer.Website.Models.SignIn
{
    public class SignInInputModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }

        public string ReturnUrl { get; set; }
    }
}