using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Controllers;

public class StockController : Controller
{
    private readonly ApplicationDbContext _context;

    public StockController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Stock (Movement Ledger)
    public async Task<IActionResult> Index(int? productId, string? type)
    {
        var query = _context.StockTransactions
            .Include(t => t.Product)
            .AsQueryable();

        if (productId.HasValue && productId.Value > 0)
        {
            query = query.Where(t => t.ProductId == productId.Value);
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(t => t.Type == type);
        }

        ViewBag.Products = new SelectList(await _context.Products.ToListAsync(), "Id", "Name", productId);
        ViewBag.Type = type;

        var transactions = await query.OrderByDescending(t => t.Date).ToListAsync();
        return View(transactions);
    }

    // GET: Stock/StockIn
    public async Task<IActionResult> StockIn(int? productId)
    {
        var viewModel = new StockTransactionViewModel
        {
            ProductId = productId ?? 0,
            Type = "Stock In",
            Products = await _context.Products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} (In Stock: {p.Quantity})"
            }).ToListAsync()
        };
        return View(viewModel);
    }

    // POST: Stock/StockIn
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StockIn(StockTransactionViewModel model)
    {
        var product = await _context.Products.FindAsync(model.ProductId);
        if (product == null)
        {
            ModelState.AddModelError("ProductId", "Selected product not found");
        }

        if (ModelState.IsValid && product != null)
        {
            product.Quantity += model.Quantity;

            _context.StockTransactions.Add(new StockTransaction
            {
                ProductId = product.Id,
                Type = "Stock In",
                Quantity = model.Quantity,
                Date = DateTime.UtcNow,
                Notes = string.IsNullOrWhiteSpace(model.Notes) ? "Restock shipment" : model.Notes
            });

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Successfully added {model.Quantity} units to {product.Name}. New Stock: {product.Quantity}";
            return RedirectToAction(nameof(Index));
        }

        model.Products = await _context.Products.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = $"{p.Name} (In Stock: {p.Quantity})"
        }).ToListAsync();
        return View(model);
    }

    // GET: Stock/StockOut
    public async Task<IActionResult> StockOut(int? productId)
    {
        var viewModel = new StockTransactionViewModel
        {
            ProductId = productId ?? 0,
            Type = "Stock Out",
            Products = await _context.Products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} (In Stock: {p.Quantity})"
            }).ToListAsync()
        };
        return View(viewModel);
    }

    // POST: Stock/StockOut
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StockOut(StockTransactionViewModel model)
    {
        var product = await _context.Products.FindAsync(model.ProductId);
        if (product == null)
        {
            ModelState.AddModelError("ProductId", "Selected product not found");
        }
        else if (product.Quantity < model.Quantity)
        {
            ModelState.AddModelError("Quantity", $"Not enough stock! Available in warehouse: {product.Quantity}");
        }

        if (ModelState.IsValid && product != null)
        {
            product.Quantity -= model.Quantity;

            _context.StockTransactions.Add(new StockTransaction
            {
                ProductId = product.Id,
                Type = "Stock Out",
                Quantity = model.Quantity,
                Date = DateTime.UtcNow,
                Notes = string.IsNullOrWhiteSpace(model.Notes) ? "Item Dispatch / Sale" : model.Notes
            });

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Successfully dispatched {model.Quantity} units from {product.Name}. Remaining Stock: {product.Quantity}";
            return RedirectToAction(nameof(Index));
        }

        model.Products = await _context.Products.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = $"{p.Name} (In Stock: {p.Quantity})"
        }).ToListAsync();
        return View(model);
    }
}
