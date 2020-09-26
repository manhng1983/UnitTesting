using System.ComponentModel.DataAnnotations;

namespace UnitTestArticle.Models
{
    public class TransferMoneyModel
    {
        [Display(Name ="Source Account Number")]
        [Required]
        [MinLength(10), MaxLength(10)]
        public string SourceAccountNumber { get; set; }

        [Display(Name = "Destination Account Number")]
        [Required]
        [MinLength(10), MaxLength(10)]
        public string DestinationAccountNumber { get; set; }

        [Display(Name = "Amount")]
        [Required]
        [Range(0.01,1000000)]
        public decimal Amount { get; set; }
    }
}