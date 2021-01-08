using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ems.Areas.Identity.Types;
using ems.Models;
using Microsoft.AspNetCore.Identity;

namespace ems.Areas.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public byte[] Picture { get; set; }
        public Designation Post { get; set; }
        public int DepartmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public Department Department { get; set; }
        
        



    }
}