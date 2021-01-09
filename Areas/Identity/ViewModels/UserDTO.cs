using System;
using System.ComponentModel.DataAnnotations;
using ems.Areas.Identity.Types;

namespace ems.Areas.Identity.ViewModels
{
    public class UserDTO
    {
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "{0} should be between {2} and {1}", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "{0} should be between {2} and {1}", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Name
        {
            get
            {
                return $"{LastName}  {FirstName}";
            }
        }
        public byte[] Photo { get; set; }
        public string[] Roles { get; set; }
        public string Department { get; set; }
        public Designation Post { get; set; }
        public string Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


    }
}