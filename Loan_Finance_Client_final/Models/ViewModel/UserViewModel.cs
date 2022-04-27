using System.ComponentModel.DataAnnotations;

namespace Loan_Finance_Client_final.Models.ViewModel
{
    public class UserViewModel
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(30)]
        public string UserName { get; set; }

        [Required, MaxLength(10)]
        public string PhoneNumber { get; set; }

        [Required, Range(23, 59)]
        public int Age { get; set; }

        [Required]
        public string Address { get; set; }

        [Required, MaxLength(10)]
        public string Pan { get; set; }

    
        public IFormFile PanFile { get; set; }

        [Required, MinLength(12), MaxLength(12)]
        public string Aadhar { get; set; }

      
        public IFormFile AadharFile { get; set; }

        [Required, Range(15000, 200000)]
        public int Salary { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
