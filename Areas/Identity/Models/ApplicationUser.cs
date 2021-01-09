using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ems.Areas.Identity.Types;
using ems.Areas.Identity.ViewModels;
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
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public Department Department { get; set; }

        internal UserDTO ToDTO()
        {
            return new UserDTO
            {
                FirstName = FirstName,
                LastName = LastName,
                Address = Address,
                Email = Email,
                StartDate = CreatedDate,
                Post = Post,
                PhoneNumber = PhoneNumber,
            };
        }

        internal UserViewModel ToViewModel()
        {
            return new UserViewModel
            {
                FirstName = FirstName,
                LastName = LastName,
                Address = Address,
                Email = Email,
                StartDate = CreatedDate,
                Post = Post,
                PhoneNumber = PhoneNumber,
                Roles = UserRoles.Select(ur => ur.Role).Select(r => r.Name).ToArray()
            };
        }
    }
}