using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loan_Finance_Client_final.Models
{
    public class Loan
    {
        [Key]
        public int LoanID { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public virtual User user { get; set; }

        [Required]
        public string VechicleType { get; set; }

        [Required]
        public string VechicleModel { get; set; }

        [Required]
        public int OnRoadPrice { get; set; }

        [Required]
        public int LoanAmount { get; set; }

        [Required]
        public int MonthOfEmi { get; set; }

        public string Message { get; set; }

        public string Status { get; set; } = "Processing";
    }
}
