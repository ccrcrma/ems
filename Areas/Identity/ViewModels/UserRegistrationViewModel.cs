using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using ems.Areas.Identity.Models;
using ems.Areas.Identity.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ems.Areas.Identity.ViewModels
{
    public class UserRegistrationViewModel
    {
        public class GeneralInformation
        {

            [Required(ErrorMessage = "{0} is Required")]
            [StringLength(100, ErrorMessage = "{0} must be between {2} and {1} charactrs", MinimumLength = 2)]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "{0} is Required")]
            [StringLength(100, ErrorMessage = "{0} must be between {2} and {1} charactrs", MinimumLength = 2)]

            public string LastName { get; set; }

            [Required(ErrorMessage = "{0} is Required")]
            [StringLength(200, ErrorMessage = "{0} must be between {2} and {1} charactrs", MinimumLength = 2)]

            public string Address { get; set; }

            [Required(ErrorMessage = "{0} is Required")]
            [RegularExpression(@"^98[0-9]{8}$", ErrorMessage = "Enter a 10 digit Nepali Number")]
            [Display(Name = "Mobile Number")]
            [StringLength(10, ErrorMessage = "{0} must be {1} digits", MinimumLength = 10)]
            public string MobileNumber { get; set; }


            [Display(Name = "Photo")]
            public IFormFile FormFile { get; set; }
            public int Department { get; set; }
            public Designation Post { get; set; }

            [Display(Name = "Start Date")]
            [DataType(DataType.Date)]
            public DateTime StartDate { get; set; } = DateTime.Today;
            public List<SelectListItem> Departments { get; set; } = new List<SelectListItem>();
            
            [Required(ErrorMessage = "{0} is Required")]
            public string Role { get; set; }
            public List<SelectListItem> Roles { get; set; }
        }

        public class LoginInformation
        {
            [Required(ErrorMessage = "{0} is Required")]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0} is Required")]
            [DataType(DataType.Password)]
            [StringLength(int.MaxValue, ErrorMessage = "{0} must be at least {2} characters", MinimumLength = 5)]
            public string Password { get; set; }


            [Required(ErrorMessage = "{0} is Required")]
            [DataType(DataType.Password)]
            [StringLength(int.MaxValue, ErrorMessage = "{0} must be at least {2} characters", MinimumLength = 5)]
            [Compare("Password")]
            [Display(Name = "Confirm Password")]
            public string ConfirmPassword { get; set; }
        }

        public GeneralInformation GeneralInfo = new GeneralInformation();
        public LoginInformation LoginInfo = new LoginInformation();

        public ApplicationUser ToModel()
        {
            var file = GeneralInfo.FormFile;
            byte[] fileBytes = null;
            if (file != null && file.Length > 0)
            {
                using (var memStream = new MemoryStream())
                {
                    file.CopyToAsync(memStream);
                    fileBytes = memStream.ToArray();
                }
            }

            return new ApplicationUser
            {
                FirstName = GeneralInfo.FirstName,
                LastName = GeneralInfo.LastName,
                Address = GeneralInfo.LastName,
                DepartmentId = GeneralInfo.Department,
                PhoneNumber = GeneralInfo.MobileNumber,
                Post = GeneralInfo.Post,
                CreatedDate = GeneralInfo.StartDate,
                Picture = fileBytes,
                Email = LoginInfo.Email,
                UserName = LoginInfo.Email,
            };
        }
    }
}