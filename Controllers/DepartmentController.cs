using ems.Data;
using ems.Models;
using System.Threading.Tasks;
using ems.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ems.Helpers.Alert;
using Microsoft.AspNetCore.Authorization;
using ems.Helpers.Permissions;

namespace ems.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationContext _context;

        public DepartmentController(ApplicationContext context)
        {
            _context = context;
        }

        [Authorize(Permissions.Department.Create)]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Permissions.Department.List)]
        public async Task<IActionResult> IndexAsync()
        {
            var departments = await _context.Departments.Select(d => new DepartmentViewModel
            {
                Name = d.Name,
                Description = d.Description,
                Id = d.Id

            }).ToListAsync();
            return View(departments);
        }

        [HttpGet]
        [Authorize(Permissions.Department.Edit)]
        public async Task<IActionResult> EditAsync(int id)
        {
            var departmentVm = (await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id))
                .ToViewModel();

            if (departmentVm == null)
            {
                return NotFound();
            }
            return View(departmentVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Department.Delete)]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (department == null)
            {
                return BadRequest();
            }
            _context.Entry(department).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index")
            .WithSuccess("congrats", "the department was deleted successfully");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Department.Edit)]

        public async Task<IActionResult> Edit(int id, DepartmentViewModel departmentVm)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVm);
            }

            var department = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
            if (department == null)
            {
                return BadRequest();
            }
            department = departmentVm.ToModel();
            _context.Entry(department).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Department.Create)]

        public async Task<IActionResult> CreateAsync(DepartmentViewModel departmentVm)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVm);
            }
            var department = departmentVm.ToModel();
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return LocalRedirect("~/").WithSuccess("hurray", $"new Department {department.Name} created");
            // return AlertExtensions.WithSuccess(LocalRedirect("~/"), "hurray", "$new Department {department.Name} created");
        }
    }
}