using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ems.Areas.Identity.Models;
using ems.Extensions;
using ems.Helpers.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ems.Areas.Identity.Data
{
    public static class SampleData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();
            Dictionary<string, string> roles = new Dictionary<string, string> {
                { "Admin", "powerful user"},
                { "Supervisor", "supervises everytthing" }
            };

            foreach (var roleDescription in roles)
            {
                if (!await (RoleManager.RoleExistsAsync(roleDescription.Key.ToUpperFirstChar())))
                {
                    var result = await RoleManager.CreateAsync(new ApplicationRole
                    {
                        Name = roleDescription.Key.ToUpperFirstChar(),
                        Description = roleDescription.Value
                    });
                    var role = await RoleManager.FindByNameAsync(roleDescription.Key.ToUpperFirstChar());
                    await RoleManager.AddClaimAsync(role, new Claim(CustomClaimType.Permission, Permissions.Department.Create));
                }
            }
        }
    }
}