using Microsoft.AspNetCore.Mvc;

namespace KeywordStatsWeb.Controllers
{
    public class StatController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
