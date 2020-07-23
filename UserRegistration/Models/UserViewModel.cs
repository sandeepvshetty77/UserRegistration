using System.ComponentModel.DataAnnotations;

namespace UserRegistration.Models
{
    // UserViewModel has customised details of the main User Model for display purpose. It has confirm password as addition.
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Error: Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
