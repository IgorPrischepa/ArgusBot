using System.ComponentModel.DataAnnotations;

namespace ArgusBot.Models.Account
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "Login")]
        [MinLength(5)]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }
        public string RedirectUrl { get; set; }
    }
}
