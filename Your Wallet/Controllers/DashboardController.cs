using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Your_Wallet.Areas.Identity.Data;
using Your_Wallet.Models;
using Your_Wallet.Models.Data;

namespace Your_Wallet.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MainContext _context;

        // GET: Dashboard
        public DashboardController(UserManager<ApplicationUser> userManager, MainContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            // Redirect to additional info page if user has not set their name
            var user = await _userManager.GetUserAsync(User);

            if (!user.NameSet)
            {
                return RedirectToAction("AdditionalInfo", "Account");
            }

            List<Transaction> Transactions = await _context.Transactions
                                .Include(x => x.Category)
                                .Where(x => x.ApplicationUserId == user.Id)
                                .ToListAsync();


            // Total Income
            decimal totalIncome = Transactions
                .Where(x => x.Category.Type == CategoryType.Income)
                .Sum(x => x.Amount);

            ViewBag.TotalIncome = totalIncome.ToString("C0");

            // Total Expense
            decimal totalExpense = Transactions
                .Where(x => x.Category.Type == CategoryType.Expense)
                .Sum(x => x.Amount);

            ViewBag.TotalExpense = totalExpense.ToString("C0");


            // Balance
            decimal Balance = (totalIncome + user.StartingBalance ?? 0) - totalExpense;

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);


            var Expenses = Transactions
                .Where(x => x.Category.Type == CategoryType.Expense)
                .GroupBy(j => j.Category.Id)
                .Select(k => new
                {
                    title = k.FirstOrDefault().Category.TitleDisplay,
                    totalAmmount = k.Sum(x => x.Amount),
                    AmountToDisplay = k.Sum(j => j.Amount).ToString("C0")
                })
                .OrderByDescending(l => l.totalAmmount)
                .ToList();

            ViewBag.Expenses = Expenses;



            return View();
        }
    }
}
