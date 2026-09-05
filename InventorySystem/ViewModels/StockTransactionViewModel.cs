using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventorySystem.ViewModels;

public class StockTransactionViewModel
{
    [Required(ErrorMessage = "Please select a product")]
    [Display(Name = "Product")]
    public int ProductId { get; set; }

    [Display(Name = "Movement Type")]
    public string Type { get; set; } = "Stock In";

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, 100000, ErrorMessage = "Quantity must be at least 1")]
    [Display(Name = "Quantity")]
    public int Quantity { get; set; } = 1;

    [Display(Name = "Notes / Reason")]
    [StringLength(250)]
    public string? Notes { get; set; }

    public IEnumerable<SelectListItem> Products { get; set; } = new List<SelectListItem>();
}
