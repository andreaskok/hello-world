using System.ComponentModel.DataAnnotations;

namespace ERPDomain.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Please enter email address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address.")]
        [Display(Name = "Login Email")]
        public string LoginEmail { get; set; }

        public string LoginUserID { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Login Password")]
        public string LoginPassword { get; set; }
    }
}
