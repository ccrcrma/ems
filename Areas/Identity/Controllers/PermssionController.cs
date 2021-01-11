using System.Linq;
using ems.Areas.Identity.Models;
using ems.Areas.Identity.ViewModels;
using ems.Data;
using ems.Helpers.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using ems.Helpers;
using ems.Util;
using ems.Helpers.Alert;
using System.Collections.Generic;

namespace ems.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("[Controller]")]
    public class PermissionController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationContext _context;
        private readonly ILogger<PermissionController> _logger;

        public PermissionController(RoleManager<ApplicationRole> roleManager,
            ApplicationContext context,
            ILogger<PermissionController> logger)
        {
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.Select(r => new RoleViewModel
            {
                Name = r.Name,
                Description = r.Description,
                Id = r.Id
            }).ToList();
            return View(roles);
        }

        [HttpGet]
        [Route("{id}/[Action]")]
        public async Task<IActionResult> EditAsync(string id)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == id);
            if (role == null) return BadRequest();
            var Roleclaims = await _roleManager.GetClaimsAsync(role);
            var resourcePermissionClaims = Roleclaims
                .Where(c => c.Type == CustomClaimType.Permission)
                .ToList();

            var vm = new PermissionEditModel();

            foreach (var claim in resourcePermissionClaims)
            {
                ReflectionHelpers.SetProperty(claim.Value.Replace("Permissions.", string.Empty),
                    vm, new Checkbox() { Text = claim.Value, Selected = true, Value = claim.Value });
            }
            return View(vm);
        }

        [HttpPost]
        [Route("{id}/[Action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(string id, PermissionEditModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            var role = await _context
                .Roles
                .Include(r => r.RoleClaims)
                .FirstOrDefaultAsync(r => r.Name == id);

            if (role == null)
            {
                return BadRequest();
            }
            role.RoleClaims = new List<ApplicationRoleClaim>();
            var allPermissions = new List<Checkbox>();

            var selectedDepartmentPermissions = viewModel.Department.ReturnSelectedActions();
            ListHelper<Checkbox>.AddRange(allPermissions, selectedDepartmentPermissions);

            var selectedNoticePermissions = viewModel.Notice.ReturnSelectedActions();
            ListHelper<Checkbox>.AddRange(allPermissions, selectedNoticePermissions);


            var selectedLeavePermissions = viewModel.Leave.ReturnSelectedActions();
            ListHelper<Checkbox>.AddRange(allPermissions, selectedLeavePermissions);

            var selectedRolePermissions = viewModel.Role.ReturnSelectedActions();
            ListHelper<Checkbox>.AddRange(allPermissions, selectedRolePermissions);

            var selectedUserPermissions = viewModel.User.ReturnSelectedActions();
            ListHelper<Checkbox>.AddRange(allPermissions, selectedUserPermissions);

            var selectedPermissionForPermissions = viewModel.Permission.ReturnSelectedActions();
            ListHelper<Checkbox>.AddRange(allPermissions, selectedPermissionForPermissions);

            foreach (var permission in allPermissions)
            {
                role.RoleClaims.Add(new ApplicationRoleClaim
                {
                    ClaimType = CustomClaimType.Permission,
                    ClaimValue = permission.Value
                });
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", new
            {
                id = id
            }).WithSuccess(string.Empty, $"permissions for {id} updated");
        }
    }
}