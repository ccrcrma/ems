using Microsoft.AspNetCore.Mvc;

namespace ems.Controllers
{
    public class NoticeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}