using System.ComponentModel.DataAnnotations;

namespace MPBankMiniProject.Models.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid Email Address.")]
        [MinLength(6, ErrorMessage = "Email Address must be at least 6 characters long.")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 25 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
