using System.ComponentModel.DataAnnotations;
using Microsoft.DotNet.Scaffolding.Shared;

namespace MPBankMiniProject.Models.ViewModels
{
    public class EditViewModel
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

        [Required]
        public float Balance { get; set; }

        [Display(Name = "Profile Picture")]
        public string? Img { get; set; }

        [Required]
        public string Id { get; set; }

        
        public IFormFile? NewImg { get; set; }
    }
}

