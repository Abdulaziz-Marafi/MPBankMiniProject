using System.ComponentModel.DataAnnotations;

namespace MPBankMiniProject.Models.ViewModels
{
    public class TransferViewModel
    {
        public string Id { get; set; } 
        public string TransferieId{ get; set; }

        [Required(ErrorMessage = "Please Enter the Amount.")]
        [Range(0.01, 100000, ErrorMessage = "Amount Selected is Ineligible.")]
        public float Amount { get; set; } 

    }
}
