using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using ems.Areas.Identity.Models;
using ems.Areas.Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ems.Extensions;
using ems.Helpers.Alert;
using Microsoft.EntityFrameworkCore;
using ems.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ems.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("[Controller]")]
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationContext _context;

        public RoleController(ILogger<RoleController> logger,
            RoleManager<ApplicationRole> roleManager,
            ApplicationContext context)
        {
            _context = context;
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("Roles")]
        public async Task<IActionResult> IndexAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [Route("[Action]")]
        [HttpGet]
        public IActionResult Create()
        {
            if (Request.Method == HttpMethod.Post.ToString())
            {
                foreach (var item in Request.Form)
                {
                    _logger.LogInformation($"{item.Key}, {Request.Form[item.Key]}");
                }

            }
            return View();
        }


        [HttpPost]
        [Route("[Action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(RoleViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                return View(roleVm);
            }
            var roleName = roleVm.Name.ToUpperFirstChar();
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                ModelState.AddModelError("Name", $"Role Named {roleName} already exists");
            }
            var result = await _roleManager.CreateAsync(new ApplicationRole
            {
                Name = roleName,
                Description = roleVm.Description
            });

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View(roleVm);
                }
            }

            return RedirectToAction("Index").WithSuccess(string.Empty, $"new {roleName} created Successfully");
        }

        [HttpGet]
        [Route("[Action]/{id}")]
        public async Task<IActionResult> EditAsync(string id)
        {
            if (id == null) return BadRequest();
            var role = await _context.Roles
                .Include(r => r.UserRoles)
                    .ThenInclude(ur => ur.User)
                .FirstOrDefaultAsync(r => r.Name == id);

            if (role == null) return NotFound();

            var roleVm = role.ToViewModel();
            var allUsersInRole = role.UserRoles.Select(ur => ur.User).Select(u => u.Id).ToArray();
            var allUserOptions = await _context.Users.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToListAsync();
            roleVm.CreateCheckBoxes(allUserOptions);
            roleVm.SetCheckBoxes(allUsersInRole);

            return View(roleVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Action]/{id}")]
        public async Task<IActionResult> EditAsync(string id, RoleViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                return View(roleVm);
            }
            var role = await _context.Roles
                .Include(r => r.UserRoles)
                .FirstOrDefaultAsync(r => r.Name == id);
            if (role == null) return BadRequest();
            var roleWithSameName = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleVm.Name.ToUpperFirstChar());
            if (roleWithSameName != null && role != roleWithSameName)
            {
                ModelState.AddModelError("Name", $"role named {roleVm.Name} already exists");
                return View(roleVm);
            }
            role = roleVm.ToModel(role);
            role.UserRoles = new List<UserRole>();
            var usersSelectedInRole = roleVm.Users.Where(u => u.Selected);
            foreach (var user in usersSelectedInRole)
            {
                role.UserRoles.Add(new UserRole
                {
                    UserId = user.Value
                });
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "Database update exception occured");
                return View(roleVm);
            }
            return RedirectToAction("Index").WithSuccess(string.Empty, $"role {roleVm.Name} updated successfully");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Action]/{id}", Name = "DeleteRole")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return BadRequest();
            var role = await _context.Roles
                .Include(r => r.UserRoles)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (role == null) return BadRequest();
            if (role.UserRoles.Count > 0)
            {
                return RedirectToAction("Index").WithDanger("Error", $"{role.Name} can't be deleted because there are {role.UserRoles.Count} users in this role");
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index").WithSuccess(string.Empty, $"Role  {role.Name} deleted successfully");
            }
            else
            {
                return RedirectToAction("Index").WithDanger("Error", "Role deletion failed ");
            }
        }
    }
}