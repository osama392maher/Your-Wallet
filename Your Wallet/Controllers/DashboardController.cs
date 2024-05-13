using Microsoft.AspNetCore.Mvc;

namespace Your_Wallet.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
