using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public class DashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalCategories { get; set; }
    public int TotalSuppliers { get; set; }
    public int TotalStock { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public int LowStockCount { get; set; }
    public int OutOfStockCount { get; set; }

    public IEnumerable<Product> LowStockProducts { get; set; } = new List<Product>();
    public IEnumerable<StockTransaction> RecentTransactions { get; set; } = new List<StockTransaction>();
}
