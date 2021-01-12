using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ems.Models;
using ems.Data;
using Microsoft.EntityFrameworkCore;
using ems.ViewModels;
using System.Security.Claims;
using static ems.ViewModels.IndexViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ems.Areas.Identity.Models;

namespace ems.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _logger = logger;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var departmentCount = await _context.Departments.CountAsync();
            var leaveCount = await _context.Leaves.CountAsync();
            var noticeCount = await _context.Notices.CountAsync();
            var userCount = await _context.Users.CountAsync();
            var indexViewModel = new IndexViewModel
            {
                DepartmentCount = departmentCount,
                LeaveCount = leaveCount,
                NoticeCount = noticeCount,
                UserCount = userCount
            };

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users
                .Include(u => u.Department)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
            indexViewModel.IndividualInfo = new PersonalDetail
            {
                Email = user.Email,
                Address = user.Address,
                MobileNumber = user.PhoneNumber,
                Designation = user.Post.ToString(),
                StartDate = user.CreatedDate,
                Department = user.Department?.Name.ToString(),
                Roles = user.UserRoles?.Select(u => u.Role).Select(r => r.Name).ToArray()
            };
            return View(indexViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult Home()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
