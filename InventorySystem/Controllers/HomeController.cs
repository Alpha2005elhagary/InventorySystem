using InventorySystem.Data;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.Include(p => p.Category).ToListAsync();
        var lowStockList = products.Where(p => p.Quantity <= p.MinStock && p.Quantity > 0).ToList();

        var viewModel = new DashboardViewModel
        {
            TotalProducts = products.Count,
            TotalCategories = await _context.Categories.CountAsync(),
            TotalSuppliers = await _context.Suppliers.CountAsync(),
            TotalStock = products.Sum(p => p.Quantity),
            TotalInventoryValue = products.Sum(p => p.Price * p.Quantity),
            LowStockCount = lowStockList.Count,
            OutOfStockCount = products.Count(p => p.Quantity <= 0),
            LowStockProducts = lowStockList,
            RecentTransactions = await _context.StockTransactions
                .Include(t => t.Product)
                .OrderByDescending(t => t.Date)
                .Take(7)
                .ToListAsync()
        };

        return View(viewModel);
    }
}
