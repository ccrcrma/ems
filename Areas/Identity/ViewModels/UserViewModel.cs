using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ems.Areas.Identity.Types;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ems.Areas.Identity.ViewModels
{
    public class UserViewModel
    {
        public class RoleCheckbox
        {
            public string Text { get; set; }
            public string Value { get; set; }
            public bool Selected { get; set; }
        }

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

        [Required(ErrorMessage = "{0} is Required")]
        public int Department { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public Designation Post { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "{0} must be between {2} and {1}")]
        public string Address { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Joined")]
        public DateTime StartDate { get; set; }
        public string Id { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public string PhoneNumber { get; set; }

        public string[] Roles;
        public List<SelectListItem> Departments { get; set; }
        public List<RoleCheckbox> RoleCheckboxes { get; set; }


    }
}