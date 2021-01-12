using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ems.Data;
using ems.Helpers.Alert;
using ems.Helpers.Permissions;
using ems.Models;
using ems.Services;
using ems.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static ems.ViewModels.MailViewModel;

namespace ems.Controllers
{
    [Route("[controller]")]
    [Authorize(Permissions.Mail)]
    public class MailController : Controller
    {
        private readonly ILogger<MailController> _logger;
        private readonly IMailService _mailService;
        private readonly ApplicationContext _context;

        public MailController(
            ILogger<MailController> logger,
            IMailService mailService,
            ApplicationContext context)
        {
            _logger = logger;
            _mailService = mailService;
            _context = context;
        }

        public async Task<IActionResult> CreateAsync()
        {
            var departments = await _context.Departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString()
            }).ToListAsync();

            var users = new List<SelectListItem>()
            {
              new  SelectListItem{Text="User1", Value="randomGuid1"},
              new  SelectListItem{Text="User2", Value="randomGuid2"},
              new  SelectListItem{Text="User3", Value="randomGuid3"},
              new  SelectListItem{Text="User3", Value="randomGuid4"},
            };

            var mailViewModel = new MailViewModel();
            mailViewModel.Departments = departments;
            mailViewModel.Users = users;
            return View(mailViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(MailViewModel Vm)
        {
            if (!ModelState.IsValid)
            {
                return View(Vm);
            }
            List<string> toEmails = new List<string>();
            switch (Vm.Receiver)
            {
                case ReceiverType.All:
                    toEmails = new List<string>() { "fetch_first_user", "fetch_second_user", "fetch_third_user", "fetch_all" };
                    break;
                case ReceiverType.Department:
                    var departmentId = Vm.ResourceId;
                    toEmails = new List<string>() { "dept_user1", "dept_user2", "dept_user3" };
                    break;
                case ReceiverType.Staff:
                    var staffId = Vm.ResourceId;
                    toEmails = new List<string>() { "teststaff@test.com" };
                    break;

            }
            try
            {
                var mailRequest = new MailRequest
                {
                    Body = Vm.Body,
                    Subject = Vm.Subject,
                    ToEmails = toEmails.ToArray(),
                    Attachments = Vm.Files

                };
                await _mailService.SendMailAsync(mailRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
            _logger.LogInformation("sending mail");
            return LocalRedirect("~/").WithSuccess(string.Empty, $"mail send successfully");
        }

    }
}