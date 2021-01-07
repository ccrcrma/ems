using ems.Data;
using ems.Helpers.Alert;
using ems.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ems.Controllers
{
    public class NoticeController : Controller
    {
        private readonly ApplicationContext _context;

        public NoticeController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var notices = await _context.Notices.Select(n => n.ToViewModel()).AsNoTracking().ToListAsync();
            return View(notices);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var notice = (await _context.Notices.FirstOrDefaultAsync(n => n.Id == id)).ToViewModel();
            return View(notice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, NoticeViewModel noticeVm)
        {
            if (!ModelState.IsValid)
            {
                return View(noticeVm);
            }
            var notice = await _context.Notices.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
            if (notice == null)
            {
                return BadRequest();
            }
            notice = noticeVm.ToModel();
            _context.Entry(notice).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index").WithSuccess(string.Empty, $"notice with id {id} edited");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var notice = await _context.Notices.FirstOrDefaultAsync(n => n.Id == id);
            if (notice == null)
                return BadRequest();
            _context.Entry(notice).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index").WithSuccess(string.Empty, $"notice with id {id} deleted");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(NoticeViewModel noticeVm)
        {
            if (!ModelState.IsValid)
            {
                return View(noticeVm);
            }
            var notice = noticeVm.ToModel();
            _context.Notices.Add(notice);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home").WithSuccess("", "new notice created");

        }

    }
}