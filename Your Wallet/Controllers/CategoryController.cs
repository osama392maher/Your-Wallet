using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Your_Wallet.Models;
using Your_Wallet.Models.Data;

namespace Your_Wallet.Controllers;

public class CategoryController : Controller
{
    private readonly MainContext _context;

    public CategoryController(MainContext context)
    {
        _context = context;
    }

    // GET: Category
    public async Task<IActionResult> Index()
    {
        return View(await _context.Categories.ToListAsync());
    }
    
    // GET: Category/Create
    public IActionResult Create()
    {
        return View("AddForm", new Category());
    }

    // POST: Category/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromRoute] int id, Category category)
    {
        if (ModelState.IsValid)
        {
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

        return View("AddForm", category);
    }

    // GET: Category/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        return View("AddForm", category);
    }
    
    // POST: Category/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null) _context.Categories.Remove(category);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id);
    }
}