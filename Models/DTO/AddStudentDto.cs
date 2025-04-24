using System.ComponentModel.DataAnnotations;

namespace PSchoolApp.Models.DTO
{
    public class AddStudentDto
    {
        [Required(ErrorMessage = "First Name is required")]
        public string F_Name { get; set; }

        [Required(ErrorMessage = "Second Name is required")]
        public string S_Name { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parent ID is required")]
        public Guid ParentID { get; set; }
    }
}
