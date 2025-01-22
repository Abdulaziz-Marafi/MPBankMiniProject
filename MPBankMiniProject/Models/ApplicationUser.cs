using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MPBankMiniProject.Models
{
    public class ApplicationUser : IdentityUser 
    {
        [Required(ErrorMessage ="Please Enter Your First Name")]
        [Display(Name ="First Name")]
        [MinLength(2, ErrorMessage = "Your First Name Must Be At Least 2 Charachters Long")]
        public string FName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please Enter Your Last Name")]
        [MinLength(2, ErrorMessage = "Your Last Name Must Be At Least 2 Charachters Long")]
        public string LName { get; set; }

        [Required]
        public float Balance { get; set; } = 100000;
        //public byte[]? Img { get; set; }

        public string? Img { get; set; }

        public IList<Transaction>? Transactions { get; set; }
    }
}
