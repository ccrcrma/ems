using Microsoft.AspNetCore.Mvc;

namespace ems.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}