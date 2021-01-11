using System.Collections.Generic;
using ems.Areas.Identity.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ems.Areas.Identity.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }

        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public List<ApplicationRoleClaim> RoleClaims { get; set; } = new List<ApplicationRoleClaim>();

        public RoleViewModel ToViewModel()
        {
            return new RoleViewModel
            {
                Name = Name,
                Description = Description,

            };
        }


    }
}