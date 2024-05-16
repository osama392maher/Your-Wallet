using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Your_Wallet.Areas.Identity.Data;
using Your_Wallet.Models;
using Your_Wallet.Models.Data;
using Your_Wallet.Models.ViewModels;

namespace Your_Wallet.Controllers;

[Authorize]
public class CategoryController : Controller
{
    private readonly MainContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public CategoryController(MainContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
  

    // GET: Category
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (!user.NameSet)
        {
            return RedirectToAction("AdditionalInfo", "Account");
        }

        var categories = await _context.Categories
    .Where(c => c.ApplicationUserId == user.Id)
    .ToListAsync();

        return View(categories);
    }
    
    // GET: Category/Create
    public async Task<IActionResult> CreateAsync()
    {

        var user = await _userManager.GetUserAsync(User);

        if (!user.NameSet)
        {
            return RedirectToAction("AdditionalInfo", "Account");
        }
        return View("AddForm", new CategoryViewModel());
    }

    // POST: Category/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromRoute] int id, CategoryViewModel categoryVm)
    {
        var user = await _userManager.GetUserAsync(User);

         if (ModelState.IsValid)
        {

            var category = (Category)categoryVm;

            category.ApplicationUserId = user.Id;
            // if the category already exists, update it
            if (CategoryExists(category.Id))
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                        return NotFound();
                    throw;
                }
            }
            else // if the category does not exist, add it
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        return View("AddForm", categoryVm);
    }

    // GET: Category/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        var user = await _userManager.GetUserAsync(User);

        if (!user.NameSet)
        {
            return RedirectToAction("AdditionalInfo", "Account");
        }

        if (id == null) return NotFound();

        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        var categoryVm = (CategoryViewModel)category;

        return View("AddForm", categoryVm);
    }
    
    // POST: Category/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _userManager.GetUserAsync(User);

        var category = await _context.Categories
            .Where(c => c.ApplicationUserId == user.Id && c.Id == id)
            .FirstOrDefaultAsync();

        if (category != null) _context.Categories.Remove(category);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CategoryExists(int id)
    {
        var user = _userManager.GetUserAsync(User).Result;
        return _context.Categories.Any(e => e.Id == id && e.ApplicationUserId == user.Id);
    }
}