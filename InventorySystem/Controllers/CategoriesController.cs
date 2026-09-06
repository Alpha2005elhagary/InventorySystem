using InventorySystem.Data;
using InventorySystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Controllers;

public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories.Include(c => c.Products).ToListAsync();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
        if (category != null)
        {
            if (category.Products.Any())
            {
                TempData["Error"] = "Cannot delete category with assigned products.";
                return RedirectToAction(nameof(Index));
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
