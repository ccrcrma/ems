using System.ComponentModel.DataAnnotations;

namespace ems.Areas.Identity.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "{0} is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [DataType(DataType.Password)]
        [StringLength(int.MaxValue, ErrorMessage = "{0} must be greater than {2}", MinimumLength = 5)]
        public string Password { get; set; }

        [Display(Name="Remember Me")]
        public bool RememberMe { get; set; }
        
        


    }
}