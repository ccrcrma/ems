using ems.Data;
using ems.Models;
using System.Threading.Tasks;
using ems.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ems.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationContext _context;

        public DepartmentController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(DepartmentViewModel departmentVm)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVm);
            }
            var department = departmentVm.ToModel();
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return LocalRedirect("~/");
        }
    }
}