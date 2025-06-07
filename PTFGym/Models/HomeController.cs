using Microsoft.AspNetCore.Mvc;

namespace PTFGym.Models
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
