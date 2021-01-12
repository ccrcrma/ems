using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ems.Data;
using ems.Handlers;
using ems.Helpers;
using static ems.Helpers.LeaveRequirement.LeaveRequirementHelpers;
using ems.Helpers.Alert;
using ems.Helpers.Permissions;
using ems.Models;
using ems.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IAuthorizationService _authorizationService;

        public LeaveController(ApplicationContext context, ILogger<LeaveController> logger, IAuthorizationService authorizationService)
        {
            _context = context;
            _logger = logger;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            LeaveCreateIndexViewModel vm = new LeaveCreateIndexViewModel();
            vm.MyLeaves = await GetMyLeaves();
            return View(vm);
        }


        private async Task<List<LeaveViewModel>> GetMyLeaves()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var allLeaves = await _context.Leaves
                .Include(l => l.Reply)
                .Include(l => l.Owner)
                .Where(l => l.OwnerId == ownerId)
                .Select(l => l.ToViewModel())
                .ToListAsync();
            return allLeaves;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([Bind(Prefix = "LeaveViewModel")] LeaveViewModel leaveVm)
        {
            LeaveCreateIndexViewModel vm = new LeaveCreateIndexViewModel();
            vm.LeaveViewModel = leaveVm;

            if (!ModelState.IsValid)
            {
                vm.MyLeaves = await GetMyLeaves();
                return View(vm);

            }
            var leave = leaveVm.ToModel();
            leave.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Leaves.Add(leave);
            await _context.SaveChangesAsync();
            return LocalRedirect("~/")
                .WithSuccess("hurray", $"new {EnumHelper<LeaveType>.GetDisplayValue(leave.Type)} created");
        }

        [HttpGet]
        [Authorize(Permissions.Leave.List)]
        public async Task<IActionResult> IndexAsync()
        {
            var leaves = await _context.Leaves
                .Include(l => l.Owner)
                .Include(l => l.Reply)
                .Select(leave => leave.ToViewModel())
                .AsNoTracking()
                .ToListAsync();
            return View(leaves);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var leave = await _context.Leaves
                .Include(l => l.Reply)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (leave == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, leave, GetRequirementForLeave(leave));
            if (authorizationResult.Succeeded)
            {
                var leaveVm = leave.ToViewModel();
                return View(leaveVm);
            }
            else if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, LeaveViewModel leaveVm)
        {
            if (!ModelState.IsValid)
            {
                return View(leaveVm);
            }
            var leave = await _context.Leaves
                .Include(l => l.Owner)
                .Include(l => l.Reply)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leave == null)
            {
                return BadRequest();
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, leave, GetRequirementForLeave(leave));
            if (authorizationResult.Succeeded)
            {

                leave = leaveVm.ToModel();
                leave.OwnerId  =User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Entry(leave).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home").WithSuccess("hurray", $"leave with id {leave.Id} was updated");
            }
            else if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var leave = await _context.Leaves
                .Include(l => l.Owner)
                .Include(l => l.Reply)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (leave == null)
                return BadRequest();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, leave, GetRequirementForLeave(leave));
            if (authorizationResult.Succeeded)
            {
                _context.Entry(leave).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return RedirectToAction("Create").WithSuccess("", $"leave with id {id} deleted successfully");

            }
            else if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }

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