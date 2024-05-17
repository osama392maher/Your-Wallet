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


            //Spline Chart - Income vs Expense

            //Income
            List<SplineChartData> IncomeSummary = Transactions
                .Where(x => x.Category.Type == CategoryType.Income)
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();

            //Expense
            List<SplineChartData> ExpenseSummary = Transactions
                .Where(x => x.Category.Type == CategoryType.Expense)
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();

            //Last 7 Days
            DateTime StartDate = DateTime.Today.AddDays(-6);

            //Combine Income & Expense
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from day in Last7Days
                                      join income in IncomeSummary on day equals income.day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,
                                      };



            //Recent Transactions
            ViewBag.RecentTransactions = Transactions
                .OrderByDescending(j => j.Date)
                .Take(5);
            



            return View();
        }
    }

    public class SplineChartData
    {
        public string day;
        public decimal income;
        public decimal expense;

    }

}

