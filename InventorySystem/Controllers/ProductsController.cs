using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Controllers;

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Products
    public async Task<IActionResult> Index(string? search, int? categoryId, bool? lowStock)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term));
        }

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (lowStock.HasValue && lowStock.Value)
        {
            query = query.Where(p => p.Quantity <= p.MinStock);
        }

        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", categoryId);
        ViewBag.Search = search;
        ViewBag.LowStock = lowStock ?? false;

        var products = await query.OrderBy(p => p.Name).ToListAsync();
        return View(products);
    }

    // GET: Products/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Include(p => p.StockTransactions.OrderByDescending(t => t.Date))
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return NotFound();
        return View(product);
    }

    // GET: Products/Create
    public async Task<IActionResult> Create()
    {
        var viewModel = new ProductViewModel
        {
            Categories = await _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync(),
            Suppliers = await _context.Suppliers.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToListAsync()
        };
        return View(viewModel);
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Quantity = model.Quantity,
                MinStock = model.MinStock,
                CategoryId = model.CategoryId,
                SupplierId = model.SupplierId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            if (model.Quantity > 0)
            {
                _context.StockTransactions.Add(new StockTransaction
                {
                    ProductId = product.Id,
                    Type = "Stock In",
                    Quantity = model.Quantity,
                    Date = DateTime.UtcNow,
                    Notes = "Initial Stock Setup"
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        model.Categories = await _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync();
        model.Suppliers = await _context.Suppliers.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToListAsync();
        return View(model);
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        var viewModel = new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity,
            MinStock = product.MinStock,
            CategoryId = product.CategoryId,
            SupplierId = product.SupplierId,
            Categories = await _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync(),
            Suppliers = await _context.Suppliers.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToListAsync()
        };
        return View(viewModel);
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel model)
    {
        if (id != model.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Name = model.Name;
            product.Price = model.Price;
            product.MinStock = model.MinStock;
            product.CategoryId = model.CategoryId;
            product.SupplierId = model.SupplierId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        model.Categories = await _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync();
        model.Suppliers = await _context.Suppliers.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToListAsync();
        return View(model);
    }

    // POST: Products/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
