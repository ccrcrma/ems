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
using static ems.Areas.Identity.ViewModels.UserViewModel;
using ems.Util;
using Microsoft.AspNetCore.Authorization;
using ems.Helpers.Permissions;

namespace ems.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("[Controller]")]
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            ApplicationContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [Route("[Action]")]
        [Authorize(Permissions.User.Create)]
        public async Task<IActionResult> CreateAsync()
        {
            var vm = new UserRegistrationViewModel();
            vm = (UserRegistrationViewModel)await populateDropDownAsync(vm);
            return View(vm);
        }

        private async Task<object> populateDropDownAsync<T>(T vm)
        {
            var departmentOptions = await _context.Departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString()
            }).ToListAsync();

            var roles = await _roleManager.Roles.ToListAsync();

            var roleOptions = roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            if (vm is UserRegistrationViewModel newVm)
            {
                newVm.GeneralInfo.Roles = roleOptions;
                newVm.GeneralInfo.Departments = departmentOptions;
                return newVm;
            }
            else if (vm is UserViewModel userVm)
            {
                userVm.Departments = departmentOptions;
                userVm.CreateCheckBoxes(roleOptions);
                return userVm;
            }
            throw new ArgumentException("Invalid Argument");

        }

        [HttpPost("[Action]")]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.User.Create)]
        public async Task<IActionResult> CreateAsync(
            [Bind(Prefix = "GeneralInfo")] UserRegistrationViewModel.GeneralInformation generalInfo,
            [Bind(Prefix = "LoginInfo")] UserRegistrationViewModel.LoginInformation loginInfo)
        {
            UserRegistrationViewModel userVm = new UserRegistrationViewModel();
            userVm.GeneralInfo = generalInfo;
            userVm.LoginInfo = loginInfo;
            if (!ModelState.IsValid)
            {
                userVm = (UserRegistrationViewModel)await populateDropDownAsync(userVm);
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

        [HttpGet("Users")]
        [Authorize(Permissions.User.List)]
        public async Task<IActionResult> IndexAsync()
        {
            var users = await _context.Users
                .Include(u => u.Department)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Select(u => new UserDTO
                {
                    Photo = u.Picture,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Roles = u.UserRoles.Select(ur => ur.Role).Select(r => r.Name).ToArray(),
                    Department = u.Department.Name,
                    Post = u.Post,
                    StartDate = u.CreatedDate,
                    Address = u.Address,
                    PhoneNumber = u.PhoneNumber,
                    Id = u.Id
                }).ToListAsync();
            return View(users);
        }

        [Route("[Action]/{id}")]
        [Authorize(Permissions.User.Edit)]
        [HttpGet]
        public async Task<IActionResult> EditAsync(string id)
        {
            var user = (await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(u => u.Id == id));

            if (user == null) return NotFound();

            var userRoles = user.UserRoles?.Select(ur => ur.Role).Select(r => r.Id).ToArray();
            var userVm = user.ToViewModel();
            userVm = (UserViewModel)await populateDropDownAsync(userVm);
            userVm.SetCheckBoxes(userRoles);
            return View(userVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Action]/{id}")]
        [Authorize(Permissions.User.Edit)]

        public async Task<IActionResult> EditAsync(string id, UserViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var user = await _context.Users
                .Include(u => u.Department)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return BadRequest();

            user.DepartmentId = vm.Department;
            user.Post = vm.Post;
            user.CreatedDate = vm.StartDate;

            user.UserRoles = new List<UserRole>();
            var selectedRoles = vm.Roles.Where(r => r.Selected == true).Select(r => r.Value).ToList();
            var roles = await _roleManager.Roles.Where(r => selectedRoles.Contains(r.Id)).ToListAsync();
            foreach (var role in roles)
            {
                user.UserRoles.Add(new UserRole { Role = role });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index").WithSuccess(string.Empty, "user modified successfully");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Action]/{id}", Name = "DeleteUser")]
        [Authorize(Permissions.User.Delete)]

        public async Task<IActionResult> DeleteAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return BadRequest();
            _context.Entry(user).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index").WithSuccess(string.Empty, $"user  {user.LastName} {user.FirstName} deleted ");
        }

        [HttpGet]
        [Route("[Action]")]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost("[Action]")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SignInAsync(SignInViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { Area = "" });
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Account is Locked Out");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "Not allowed");
                }
            }

            ModelState.AddModelError(string.Empty, "Email and Password not correct");
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect("~/");
        }
    }

}