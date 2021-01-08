using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ems.Areas.Identity.Data
{
    public static class SampleData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            string[] roles = new string[] { "Admin", "Supervisor" };
            foreach (var role in roles)
            {
                if (!RoleManager.RoleExistsAsync(role).Result)
                {
                    RoleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
        }
    }
}