using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Your_Wallet.Areas.Identity.Data;
using Your_Wallet.Models.Data;

namespace Your_Wallet.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        // GET: Dashboard
        public DashboardController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> IndexAsync()
        {

            var user = await _userManager.GetUserAsync(User);

            if (!user.NameSet)
            {
                return RedirectToAction("AdditionalInfo", "Account");
            }

            ViewBag.UserName = User.Identity.Name;
            return View();
        }
    }
}
