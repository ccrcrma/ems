using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ems.Areas.Identity.Models
{
    public class UserRole : IdentityUserRole<string>
    {
        public ApplicationUser User { get; set; }
        public IdentityRole Role { get; set; }




    }
}