using System;
using System.Linq;
using System.Threading.Tasks;
using ems.Data;
using ems.Helpers;
using ems.Helpers.Alert;
using ems.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ems.Models.Leave;

namespace ems.Controllers
{
    public class LeaveController : Controller
    {
        private ApplicationContext _context;

        public LeaveController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(LeaveViewModel leaveVm)
        {
            if (!ModelState.IsValid)
                return View(leaveVm);
            var leave = leaveVm.ToModel();
            _context.Leaves.Add(leave);
            await _context.SaveChangesAsync();
            return LocalRedirect("~/").WithSuccess("hurray", $"new {EnumHelper<LeaveType>.GetDisplayValue(leave.Type)} created");
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var leaves =  await _context.Leaves.Select(leave => leave.ToViewModel()).AsNoTracking().ToListAsync();
            return View(leaves);
        }
    }
}