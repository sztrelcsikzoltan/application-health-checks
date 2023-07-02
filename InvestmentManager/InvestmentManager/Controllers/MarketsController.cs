using Microsoft.AspNetCore.Mvc;

namespace InvestmentManager.Controllers
{
    public class MarketsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}