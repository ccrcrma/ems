using System.Linq;
using ems.Areas.Identity.ViewModels;
using System.Threading.Tasks;
using ems.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using ems.Areas.Identity.Models;
using ems.Helpers.Alert;
using System;
using static ems.Areas.Identity.ViewModels.UserRegistrationViewModel;

namespace ems.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("[Controller]")]
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            ApplicationContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("[Action]")]
        public async Task<IActionResult> CreateAsync()
        {
            var vm = await populateDropDownAsync();
            return View(vm);
        }

        private async Task<UserRegistrationViewModel> populateDropDownAsync(UserRegistrationViewModel vm = null)
        {
            if (vm == null)
            {
                vm = new UserRegistrationViewModel();
            }
            var departmentOptions = await _context.Departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString()
            }).ToListAsync();
            var roleOptions = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToListAsync();
            vm.GeneralInfo.Roles = roleOptions;
            vm.GeneralInfo.Departments = departmentOptions;
            return vm;


        }

        [HttpPost("[Action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(
            [Bind(Prefix = "GeneralInfo")] UserRegistrationViewModel.GeneralInformation generalInfo,
            [Bind(Prefix = "LoginInfo")] UserRegistrationViewModel.LoginInformation loginInfo)
        {
            UserRegistrationViewModel userVm = new UserRegistrationViewModel();
            userVm.GeneralInfo = generalInfo;
            userVm.LoginInfo = loginInfo;
            if (!ModelState.IsValid)
            {
                userVm = await populateDropDownAsync(userVm);
                return View(userVm);
            }
            var user = userVm.ToModel();
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == userVm.GeneralInfo.Role);
            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "the provided role doesn't exist");
                return View(userVm);
            }
            var userCreationResult = await _userManager.CreateAsync(user, userVm.LoginInfo.Password);
            if (!userCreationResult.Succeeded)
            {
                foreach (var error in userCreationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View(userVm);
                }
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View(userVm);
                }
            }


            return RedirectToAction("Index").WithSuccess(string.Empty, $"new {role.Name} created successfully");

        }

        [HttpGet("users")]
        public async Task<IActionResult> IndexAsync()
        {
            var users = await _context.Users
                .Include(u => u.Department)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Select(u => new UserViewModel
                {
                    Photo = u.Picture,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Roles = u.UserRoles.Select(ur => ur.Role).Select(r => r.Name).ToArray(),
                    Department = u.Department.Name,
                    Post = u.Post.ToString(),
                    StartDate = u.CreatedDate,
                    Address = u.Address,
                    PhoneNumber = u.PhoneNumber,
                }).ToListAsync();
            return View(users);
        }
    }
}