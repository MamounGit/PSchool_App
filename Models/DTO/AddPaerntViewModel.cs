using System.ComponentModel.DataAnnotations;

namespace PSchoolApp.Models.DTO
{
    public class AddPaerntViewModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public string F_Name { get; set; }

        [Required(ErrorMessage = "Second Name is required.")]
        [StringLength(50, ErrorMessage = "Second Name cannot be longer than 50 characters.")]
        public string S_Name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, ErrorMessage = "Phone number must be 10 digits.", MinimumLength = 10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
    }
}
