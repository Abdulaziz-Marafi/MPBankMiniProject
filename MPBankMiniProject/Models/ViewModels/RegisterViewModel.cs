using System.ComponentModel.DataAnnotations;

namespace MPBankMiniProject.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please Enter Your First Name")]
        [Display(Name = "First Name")]
        [MinLength(2, ErrorMessage = "Your First Name Must Be At Least 2 Charachters Long")]
        public string FName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please Enter Your Last Name")]
        [MinLength(2, ErrorMessage = "Your Last Name Must Be At Least 2 Charachters Long")]
        public string LName { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid Email Address.")]
        [MinLength(6, ErrorMessage = "Email Address must be at least 6 characters long.")]        
        public string Email { get; set; }

        [Display(Name = "Confirm Email Address")]
        [Required(ErrorMessage = "Email Confirmation is Required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid Confirm Email Address.")]
        [MinLength(6, ErrorMessage = "Email Address must be at least 6 characters long.")]
        [Compare("Email", ErrorMessage = "Email and Confirm Email must match.")]
        public string ConfirmEmail { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 25 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Confirm Password must be between 8 and 25 characters")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }

        public IFormFile? Image { get; set; }
    }
}
