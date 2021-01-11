using Microsoft.AspNetCore.Identity;

namespace ems.Areas.Identity.Models
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public ApplicationRole Role { get; set; }        

    }
}