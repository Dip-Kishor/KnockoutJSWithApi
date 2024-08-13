using Microsoft.AspNetCore.Mvc;

namespace KOPracticeWithApi.Controllers
{
    public class VMController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
