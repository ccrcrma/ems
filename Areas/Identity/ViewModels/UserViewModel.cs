using System;

namespace ems.Areas.Identity.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name
        {
            get
            {
                return $"{LastName}  {FirstName}";
            }
        }
        public byte[] Photo { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public string Department { get; set; }
        public string Post { get; set; }
        public string Address { get; set; }
        public DateTime StartDate { get; set; }
        public string PhoneNumber { get; set; }
    }
}