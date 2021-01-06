using System;
using System.Linq;
using System.Threading.Tasks;
using ems.Data;
using ems.Helpers;
using ems.Helpers.Alert;
using ems.Models;
using ems.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static ems.Models.Leave;

namespace ems.Controllers
{
    public class LeaveController : Controller
    {
        private ApplicationContext _context;
        private readonly ILogger<LeaveController> _logger;

        public LeaveController(ApplicationContext context, ILogger<LeaveController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            LeaveCreateIndexViewModel vm = new LeaveCreateIndexViewModel();
            var allLeaves = await _context.Leaves.Include(l => l.Reply).Select(l => l.ToViewModel()).ToListAsync();
            vm.MyLeaves = allLeaves;
            return View(vm);
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
            var leaves = await _context.Leaves.Include(l => l.Reply).Select(leave => leave.ToViewModel()).AsNoTracking().ToListAsync();
            return View(leaves);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var leave = (await _context.Leaves.FirstOrDefaultAsync(l => l.Id == id)).ToViewModel();
            if (leave == null)
                return NotFound();
            return View(leave);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, LeaveViewModel leaveVm)
        {
            if (!ModelState.IsValid)
            {
                return View(leaveVm);
            }
            var leave = await _context.Leaves.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
            if (leave == null)
            {
                return BadRequest();
            }

            leave = leaveVm.ToModel();
            _context.Entry(leave).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home").WithSuccess("hurray", $"leave with id {leave.Id} was updated");

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var leave = await _context.Leaves.FirstOrDefaultAsync(l => l.Id == id);
            if (leave == null)
                return BadRequest();
            _context.Entry(leave).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return RedirectToAction("Create").WithSuccess("", $"leave with id {id} deleted successfully");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DecisionAsync(int id, string description, Models.Reply.ReplyStatus status)
        {
            var leave = await _context.Leaves.Include(l => l.Reply).FirstOrDefaultAsync(l => l.Id == id);
            if (leave == null)
            {
                return BadRequest();
            }
            var reply = leave.Reply;
            if (reply == null)
            {
                reply = new Reply();
                reply.LeaveId = leave.Id;
                reply.Status = status;
                reply.Description = description;
                _context.Entry(reply).State = EntityState.Added;
            }
            else
            {
                reply.Status = status;
                reply.Description = description;
                _context.Entry(reply).State = EntityState.Modified;

            }

            _logger.LogInformation(_context.Entry(reply).State.ToString());
            await _context.SaveChangesAsync();
            return RedirectToAction("Index").WithSuccess("", $"leave with id {id} {status.ToString()}");
        }
    }
}