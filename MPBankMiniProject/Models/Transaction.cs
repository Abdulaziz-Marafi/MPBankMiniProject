using System.ComponentModel.DataAnnotations;

namespace MPBankMiniProject.Models
{
    public class Transaction
    {
        [Display(Name ="Transaction ID")]
        public Guid TransactionId { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required(ErrorMessage ="Please Enter the Amount.")]
        [Range(0.01, 100000, ErrorMessage = "Amount Selected is Ineligible.")]
        public float Amount { get; set; }

        [Required]
        public DateTime TransactionDate {  get; set; } 


        public enum TransactionType
        {
            Withdraw,
            Deposit
        }
    }
}
