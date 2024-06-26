using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Your_Wallet.Areas.Identity.Data;
using Your_Wallet.Models;
using Your_Wallet.Models.Data;
using Your_Wallet.Models.ViewModels;

namespace Your_Wallet.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly MainContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionController(MainContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (!user.NameSet)
            {
                return RedirectToAction("AdditionalInfo", "Account");
            }

            var transactions = await _context.Transactions
                .Where(t => t.ApplicationUserId == user.Id)
                .Include(t => t.Category)
                .ToListAsync();

            return View(transactions);
        }
        
        
        // GET: Transaction/Create
        public async Task<IActionResult> CreateAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (!user.NameSet)
            {
                return RedirectToAction("AdditionalInfo", "Account");
            }

            ViewBag.Categories = _context.Categories
                .Where(c => c.ApplicationUserId == user.Id || c.ApplicationUserId == null)
                .ToList();
            return View("AddForm", new  TransactionViewModel());
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,Date,Note,CategoryId")] TransactionViewModel transactionVM)
        {
            if (ModelState.IsValid)
            {

                var transaction = (Transaction)transactionVM;

                var user = await _userManager.GetUserAsync(User);
                transaction.ApplicationUserId = user.Id;
                
                if (transaction.Id == 0)
                    _context.Add(transaction);
                else
                {
                    try
                    {
                        _context.Update(transaction);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TransactionExists(transaction.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View("AddForm", new TransactionViewModel());
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (!user.NameSet)
            {
                return RedirectToAction("AdditionalInfo", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _context.Categories.ToList();

            var transactionVm = (TransactionViewModel)transaction;
            return View("AddForm",transactionVm);
        }

        // POST: Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,Date,Note,CategoryId")] TransactionViewModel transactionVm)
        {
            var user = await _userManager.GetUserAsync(User);

            var transaction = (Transaction)transactionVm;
            transaction.ApplicationUserId = user.Id;

            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", transaction.CategoryId);
            return View(transactionVm);
        }
        
        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
